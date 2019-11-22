using System;
using System.Collections.Generic;
using System.IO;
using Assistant;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.UO.Network
{
    public static class IncomingPacketHandlers
    {
        private static PacketHandler[] _handlers;
        private static PacketHandler[] _extendedHandlers;

        public static void Initialize()
        {
            _handlers = new PacketHandler[0x100];
            _extendedHandlers = new PacketHandler[0x100];

            Register( 0x11, 0, OnMobileStatus );
            Register( 0x1B, 37, OnInitializePlayer );
            Register( 0x3C, 0, OnContainerContents );
            Register( 0x78, 0, OnMobileIncoming );
            Register( 0xD6, 0, OnProperties );
            Register( 0xF3, 26, OnSAWorldItem );
        }

        private static void OnProperties( PacketReader reader )
        {
            reader.Seek( 2, SeekOrigin.Current ); // word 1

            int serial = reader.ReadInt32();

            reader.Seek( 2, SeekOrigin.Current ); // word 0

            int hash = reader.ReadInt32();

            List<Property> list = new List<Property>();

            int cliloc;
            bool first = true;
            string name = "";

            while ( ( cliloc = reader.ReadInt32() ) != 0 )
            {
                Property property = new Property { Cliloc = cliloc };

                int length = reader.ReadInt16();

                property.Arguments = reader.ReadUnicodeString( length )
                    .Split( new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries );

                property.Text = Cliloc.GetLocalString( cliloc, property.Arguments );

                if ( first )
                {
                    name = property.Text;
                    first = false;
                }

                list.Add( property );
            }

            if ( Engine.Player.Serial == serial )
            {
                Engine.Player.Name = name;
                Engine.Player.Properties = list.ToArray();
            }
            else if ( UOMath.IsMobile( serial ) )
            {
                Mobile mobile = Engine.GetOrCreateMobile( serial );

                mobile.Name = name;
                mobile.Properties = list.ToArray();
            }
            else
            {
                Item item = Engine.GetOrCreateItem( serial );

                item.Name = name;
                item.Properties = list.ToArray();
            }
        }

        private static void OnMobileStatus( PacketReader reader )
        {
            long length = reader.Size;

            int serial = reader.ReadInt32();
            string name = reader.ReadString( 30 );
            int hits = reader.ReadInt16();
            int hitsMax = reader.ReadInt16();
            reader.ReadByte(); // Allow Name Change
            byte features = reader.ReadByte();

            Mobile mobile = serial == Engine.Player.Serial ? Engine.Player : Engine.GetOrCreateMobile( serial );
            mobile.Name = name;
            mobile.Hits = hits;
            mobile.HitsMax = hitsMax;

            if ( mobile is PlayerMobile playerMobile )
            {
                Engine.SetPlayer( playerMobile );
            }
        }

        private static void OnMobileIncoming( PacketReader reader )
        {
            int serial = reader.ReadInt32();
            ItemCollection container = new ItemCollection( serial, 125 );

            Mobile mobile = Engine.GetOrCreateMobile( serial );

            mobile.ID = reader.ReadInt16();
            mobile.X = reader.ReadInt16();
            mobile.Y = reader.ReadInt16();
            mobile.Z = reader.ReadSByte();
            mobile.Direction = (Direction) ( reader.ReadByte() & 0x07 );
            mobile.Hue = reader.ReadUInt16();
            mobile.Status = reader.ReadByte();
            mobile.Notoriety = reader.ReadByte();

            for ( ;; )
            {
                int itemSerial = reader.ReadInt32();

                if ( itemSerial == 0 )
                {
                    break;
                }

                Item item = Engine.GetOrCreateItem( itemSerial );
                item.Owner = serial;
                item.ID = reader.ReadUInt16();
                item.Layer = reader.ReadByte();

                item.Hue = reader.ReadUInt16();

                container.Add( item );
            }

            Engine.Items.Add( container.GetItems() );
            Engine.Mobiles.Add( mobile );
        }

        private static void OnContainerContents( PacketReader reader )
        {
            if ( reader.Size == 5 )
            {
                return;
            }

            bool oldStyle = false;

            int count = reader.ReadInt16();

            if ( ( reader.Size - 5 ) / 20 != count )
            {
                oldStyle = true;
            }

            ItemCollection container = null;

            for ( int i = 0; i < count; i++ )
            {
                int serial = reader.ReadInt32();
                int id = reader.ReadUInt16();
                reader.ReadByte(); // Item ID Offset
                int amount = reader.ReadUInt16();
                int x = reader.ReadInt16();
                int y = reader.ReadInt16();
                int grid = 0;

                if ( !oldStyle )
                {
                    grid = reader.ReadByte();
                }

                int containerSerial = reader.ReadInt32();
                int hue = reader.ReadUInt16();

                if ( container == null )
                {
                    container = new ItemCollection( containerSerial, count );
                }

                Item item = Engine.GetOrCreateItem( serial, containerSerial );

                item.ID = id;
                item.Count = amount;
                item.Owner = containerSerial;
                item.Hue = hue;
                item.Grid = grid;
                item.X = x;
                item.Y = y;

                container.Add( item );
            }
        }

        private static void OnSAWorldItem( PacketReader reader )
        {
            reader.ReadInt16(); // WORD 0x01
            byte type = reader.ReadByte(); // Data Type (0x00 = use TileData, 0x01 = use BodyData, 0x02 = use MultiData)
            int serialf3 = reader.ReadInt32();
            //Log.Info( "P Adding 0x{0:x}", serialf3 );
            Item item = Engine.GetOrCreateItem( serialf3 );
            item.ArtDataID = type;
            item.ID = reader.ReadUInt16();
            item.Direction = (Direction) reader.ReadByte();
            item.Count = reader.ReadUInt16();
            reader.ReadInt16(); // Second Amount?
            item.X = reader.ReadInt16();
            item.Y = reader.ReadInt16();
            item.Z = reader.ReadSByte();
            item.Light = reader.ReadByte();
            item.Hue = reader.ReadUInt16();
            item.Flags = reader.ReadByte();
            item.Owner = 0;

            Engine.Items.Add( item );
        }

        private static void OnInitializePlayer( PacketReader reader )
        {
            int serial = reader.ReadInt32();
            PlayerMobile mobile = new PlayerMobile( serial );

            reader.ReadInt32(); // DWORD 0

            short id = reader.ReadInt16();
            short x = reader.ReadInt16();
            short y = reader.ReadInt16();
            short z = reader.ReadInt16();
            byte direction = reader.ReadByte();

            mobile.ID = id;
            mobile.X = x;
            mobile.Y = y;
            mobile.Z = z;
            mobile.Direction = (Direction) direction;

            Engine.SetPlayer( mobile );
        }

        private static void Register( int packetId, int length, OnPacketReceive onReceive )
        {
            _handlers[packetId] = new PacketHandler( packetId, length, onReceive );
        }

        private static void RegisterExtended( int packetId, int length, OnPacketReceive onReceive )
        {
            _handlers[packetId] = new PacketHandler( packetId, length, onReceive );
        }

        internal static PacketHandler GetHandler( int packetId )
        {
            return _handlers[packetId];
        }

        private static PacketHandler GetExtendedHandler( int packetId )
        {
            return _handlers[packetId];
        }
    }
}