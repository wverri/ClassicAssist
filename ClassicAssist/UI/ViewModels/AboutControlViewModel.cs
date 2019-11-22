using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;
using Assistant;
using ClassicAssist.UI.Views;
using ClassicAssist.UO.Network.PacketFilter;
using ClassicAssist.UO.Network.Packets;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.UI.ViewModels
{
    public class AboutControlViewModel : BaseViewModel
    {
        private bool _connected;
        private DateTime _connectedTime;
        private Timer _timer;
        private int _playerSerial;
        private string _playerName;
        private int _itemCount;
        private int _mobileCount;
        private ICommand _showItemsCommand;
        private double _latency;
        private Timer _pingTimer;

        public double Latency
        {
            get => _latency;
            set => SetProperty(ref _latency, value);
        }

        public AboutControlViewModel()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;

            Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            BuildDate = $"{GetBuildDateTime( assembly ).ToLongDateString()}";

            Engine.ConnectedEvent += OnConnectedEvent;
            Engine.DisconnectedEvent += OnDisconnectedEvent;
            Engine.PlayerInitializedEvent += PlayerInitializedEvent;
            Engine.Items.CollectionChanged += ItemsOnCollectionChanged;
            Engine.Mobiles.CollectionChanged += MobilesOnCollectionChanged;
        }

        private void MobilesOnCollectionChanged( int totalcount )
        {
            MobileCount = totalcount;
        }

        private void ItemsOnCollectionChanged( int totalcount )
        {
            ItemCount = Engine.Items.GetTotalItemCount();
        }

        public ICommand ShowItemsCommand =>
            ( _showItemsCommand ?? ( _showItemsCommand = new RelayCommand( ShowItems, o => Connected ) ) );

        private static void ShowItems( object obj )
        {
            Dispatcher.CurrentDispatcher.Invoke( () =>
            {
                EntityCollectionViewer window = new EntityCollectionViewer()
                {
                    DataContext = new EntityCollectionViewerViewModel( Engine.Items )
                };

                window.Show();
            } );
        }

        public int ItemCount
        {
            get => _itemCount;
            set => SetProperty(ref _itemCount, value);
        }

        public int MobileCount
        {
            get => _mobileCount;
            set => SetProperty(ref _mobileCount, value);
        }

        private void PlayerInitializedEvent( PlayerMobile player )
        {
            PlayerSerial = player.Serial;
            PlayerName = player.Name;
        }

        public int PlayerSerial
        {
            get => _playerSerial;
            set => SetProperty(ref _playerSerial, value);
        }

        public string PlayerName
        {
            get => _playerName;
            set => SetProperty(ref _playerName, value);
        }

        public string BuildDate { get; set; }

        public bool Connected
        {
            get => _connected;
            set => SetProperty( ref _connected, value );
        }

        public DateTime ConnectedTime
        {
            get => _connectedTime;
            set => SetProperty( ref _connectedTime, value );
        }

        public string Product { get; } = "ClassicAssist";
        public string Version { get; set; }

        private void OnDisconnectedEvent()
        {
            Connected = false;

            _timer.Stop();
        }

        private void OnConnectedEvent()
        {
            Connected = true;
            ConnectedTime = DateTime.Now;

            _timer = new Timer( 1000 ) { AutoReset = true };
            _timer.Elapsed += ( sender, args ) => { NotifyPropertyChanged( nameof( ConnectedTime ) ); };
            _timer.Start();


            _pingTimer = new Timer( 3000 ) { AutoReset = true };
            _pingTimer.Elapsed += ( sender, args ) => PingServer();
            _pingTimer.Start();
        }

        private void PingServer()
        {
            _pingTimer.Interval = 30000;

            Random random = new Random();

            byte value = (byte)random.Next( 1, byte.MaxValue );

            Stopwatch sw = new Stopwatch();
            sw.Start();

            WaitEntries.WaitEntry we = Engine.WaitEntries.AddWait(
                new PacketFilterInfo( 0x73, new PacketFilterCondition( 1, new[] { value }, 1 ) ),
                WaitEntries.PacketDirection.Incoming );

            Engine.SendPacketToServer( new PingPacket( value ) );

            bool result = we.Lock.WaitOne( 5000 );

            sw.Stop();

            if ( result )
            {
                Latency = sw.ElapsedMilliseconds;
            }
        }

        internal static DateTime GetBuildDateTime( Assembly assembly )
        {
            System.Version.TryParse( FileVersionInfo.GetVersionInfo( assembly.Location ).ProductVersion,
                out Version version );

            DateTime buildDateTime =
                new DateTime( 2000, 1, 1 ).Add( new TimeSpan( TimeSpan.TicksPerDay * version.Revision ) );

            return buildDateTime;
        }
    }
}