using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using ClassicAssist.Misc;
using ClassicAssist.UI.Views;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network;
using ClassicAssist.UO.Network.PacketFilter;
using ClassicAssist.UO.Network.Packets;
using ClassicAssist.UO.Objects;
using CUO_API;

// ReSharper disable once CheckNamespace
namespace Assistant
{
    public static class Engine
    {
        public delegate void dConnected();

        public delegate void dDisconnected();

        public delegate void dPlayerInitialized( PlayerMobile player );

        private const int MAX_DISTANCE = 32;

        private static OnConnected _onConnected;
        private static OnDisconnected _onDisconnected;
        private static OnPacketSendRecv _onReceive;
        private static OnPacketSendRecv _onSend;
        private static OnGetUOFilePath _getUOFilePath;
        private static OnPacketSendRecv _sendToClient;
        private static OnPacketSendRecv _sendToServer;
        private static OnGetPacketLength _getPacketLength;
        private static ThreadQueue<Packet> _incomingQueue;
        private static OnUpdatePlayerPosition _onPlayerPositionChanged;
        private static ThreadQueue<Packet> _outgoingQueue;
        private static MainWindow _window;
        private static Thread _mainThread;
        public static string ClientPath { get; set; }
        public static bool Connected { get; set; }
        public static ItemCollection Items { get; set; } = new ItemCollection( 0 );
        public static MobileCollection Mobiles { get; set; } = new MobileCollection( Items );

        public static PlayerMobile Player { get; private set; }

        public static string StartupPath { get; set; }

        public static WaitEntries WaitEntries { get; set; }

        public static event dConnected ConnectedEvent;
        public static event dDisconnected DisconnectedEvent;
        public static event dPlayerInitialized PlayerInitializedEvent;

        public static unsafe void Install( PluginHeader* plugin )
        {
            Initialize();

            while ( !Debugger.IsAttached )
            {
                Thread.Sleep( 100 );
            }

            InitializePlugin( plugin );

            _mainThread = new Thread( () =>
            {
                _window = new MainWindow();
                _window.Show();
                Dispatcher.Run();
            } ) { IsBackground = true };

            _mainThread.SetApartmentState( ApartmentState.STA );
            _mainThread.Start();
        }

        private static unsafe void InitializePlugin( PluginHeader* plugin )
        {
            _onConnected = OnConnected;
            _onDisconnected = OnDisconnected;
            _onReceive = OnPacketReceive;
            _onSend = OnPacketSend;
            _onPlayerPositionChanged = OnPlayerPositionChanged;

            plugin->OnConnected = Marshal.GetFunctionPointerForDelegate( _onConnected );
            plugin->OnDisconnected = Marshal.GetFunctionPointerForDelegate( _onDisconnected );
            plugin->OnRecv = Marshal.GetFunctionPointerForDelegate( _onReceive );
            plugin->OnSend = Marshal.GetFunctionPointerForDelegate( _onSend );
            plugin->OnPlayerPositionChanged = Marshal.GetFunctionPointerForDelegate( _onPlayerPositionChanged );

            _getPacketLength = Utility.GetDelegateForFunctionPointer<OnGetPacketLength>( plugin->GetPacketLength );
            _getUOFilePath = Utility.GetDelegateForFunctionPointer<OnGetUOFilePath>( plugin->GetUOFilePath );
            _sendToClient = Utility.GetDelegateForFunctionPointer<OnPacketSendRecv>( plugin->Recv );
            _sendToServer = Utility.GetDelegateForFunctionPointer<OnPacketSendRecv>( plugin->Send );

            ClientPath = _getUOFilePath();

            Art.Initialize( ClientPath );
            Cliloc.Initialize( ClientPath );
            TileData.Initialize( ClientPath );
        }

        private static void OnPlayerPositionChanged( int x, int y, int z )
        {
            Player.X = x;
            Player.Y = y;
            Player.Z = z;

            Items.RemoveByDistance( MAX_DISTANCE, x, y );
            Mobiles.RemoveByDistance( MAX_DISTANCE, x, y );
        }

        public static Item GetOrCreateItem( int serial, int containerSerial = -1 )
        {
            return Items.GetItem( serial ) != null ? Items.GetItem( serial ) : new Item( serial, containerSerial );
        }

        public static Mobile GetOrCreateMobile( int serial )
        {
            return Mobiles.GetMobile( serial, out Mobile mobile ) ? mobile : new Mobile( serial );
        }

        private static void Initialize()
        {
            StartupPath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );

            if ( StartupPath == null )
            {
                throw new InvalidOperationException();
            }

            string[] assembles =
            {
                Path.Combine( StartupPath, "System.Runtime.CompilerServices.Unsafe.dll" ),
                Path.Combine( StartupPath, "ReactiveUI.dll" )
            };

            foreach ( string assembly in assembles )
            {
                Assembly.LoadFile( assembly );
            }

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            WaitEntries = new WaitEntries();

            _incomingQueue = new ThreadQueue<Packet>( ProcessIncomingQueue );
            _outgoingQueue = new ThreadQueue<Packet>( ProcessOutgoingQueue );

            IncomingPacketHandlers.Initialize();
            OutgoingPacketHandlers.Initialize();
        }

        private static void ProcessIncomingQueue( Packet packet )
        {
            PacketHandler handler = IncomingPacketHandlers.GetHandler( packet.GetPacketID() );

            int length = _getPacketLength( packet.GetPacketID() );

            handler?.OnReceive?.Invoke( new PacketReader( packet.GetPacket(), packet.GetLength(), length > 0 ) );
        }

        private static void ProcessOutgoingQueue( Packet packet )
        {
            PacketHandler handler = OutgoingPacketHandlers.GetHandler( packet.GetPacketID() );

            int length = _getPacketLength( packet.GetPacketID() );

            handler?.OnReceive?.Invoke( new PacketReader( packet.GetPacket(), packet.GetLength(), length > 0 ) );
        }

        private static Assembly OnAssemblyResolve( object sender, ResolveEventArgs args )
        {
            string assemblyname = new AssemblyName( args.Name ).Name;

            string[] searchPaths = { StartupPath, RuntimeEnvironment.GetRuntimeDirectory() };

            if ( assemblyname.Contains( "Colletions" ) )
            {
                assemblyname = "System.Collections";
            }

            foreach ( string searchPath in searchPaths )
            {
                string fullPath = Path.Combine( searchPath, assemblyname + ".dll" );

                if ( !File.Exists( fullPath ) )
                {
                    continue;
                }

                Assembly assembly = Assembly.LoadFrom( fullPath );
                return assembly;
            }

            return null;
        }

        public static void SetPlayer( PlayerMobile mobile )
        {
            Player = mobile;

            PlayerInitializedEvent?.Invoke( mobile );
        }

        public static void SendPacketToServer( byte[] packet, int length )
        {
            _sendToServer( ref packet, ref length );
        }

        public static void SendPacketToServer( Packets packet )
        {
            byte[] data = packet.ToArray();

            SendPacketToServer( data, data.Length );
        }

        #region UI Code

        #endregion

        #region ClassicUO Events

        private static bool OnPacketSend( ref byte[] data, ref int length )
        {
            _outgoingQueue.Enqueue( new Packet( data, length ) );

            return true;
        }

        private static bool OnPacketReceive( ref byte[] data, ref int length )
        {
            _incomingQueue.Enqueue( new Packet( data, length ) );

            WaitEntries.CheckWait( data, WaitEntries.PacketDirection.Incoming );

            return true;
        }

        private static void OnConnected()
        {
            Connected = true;

            ConnectedEvent?.Invoke();
        }

        private static void OnDisconnected()
        {
            Connected = false;

            Items.Clear();
            Mobiles.Clear();

            DisconnectedEvent?.Invoke();
        }

        #endregion
    }
}