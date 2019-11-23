using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Data.Skills;
using ClassicAssist.Resources;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network.PacketFilter;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.UO
{
    public class Commands
    {
        public enum MobileQueryType : byte
        {
            StatsRequest = 4,
            SkillsRequest = 5
        }

        public static void DragItem( int serial, int amount )
        {
            PacketWriter pw = new PacketWriter( 7 );
            pw.Write( (byte) 0x07 );
            pw.Write( serial );
            pw.Write( (short) amount );

            Engine.SendPacketToServer( pw );
        }

        public static void DropItem( int serial, int containerSerial )
        {
            PacketWriter pw = new PacketWriter( 15 );
            pw.Write( (byte) 0x08 );
            pw.Write( serial );
            pw.Write( (short) 0xFF );
            pw.Write( (short) 0xFF );
            pw.Write( (sbyte) 0 );
            pw.Write( (byte) 0 );
            pw.Write( containerSerial );

            Engine.SendPacketToServer( pw );
        }

        public static async Task DragDropAsync( int serial, int amount, int containerSerial )
        {
            await Task.Run( async () =>
            {
                DragItem( serial, amount );

                await Task.Delay( Options.CurrentOptions.ActionDelayMS );

                DropItem( serial, containerSerial );
            } );
        }

        public static void EquipItem( Item item, Layer layer )
        {
            int containerSerial = Engine.Player?.Serial ?? 0;

            if ( layer == Layer.Invalid )
            {
                StaticTile tileData = TileData.GetStaticTile( item.ID );
                layer = (Layer) tileData.Quality;
            }

            if ( layer == Layer.Invalid )
            {
                throw new ArgumentException( "EquipItem: Layer is invalid" );
            }

            PacketWriter pw = new PacketWriter( 10 );

            pw.Write( (byte) 0x13 );
            pw.Write( item.Serial );
            pw.Write( (byte) layer );
            pw.Write( containerSerial );

            DragItem( item.Serial, 1 );

            Engine.SendPacketToServer( pw );
        }

        public static void SystemMessage( string text )
        {
            byte[] textBytes = Encoding.BigEndianUnicode.GetBytes( text + '\0' );

            int length = 48 + textBytes.Length;

            PacketWriter pw = new PacketWriter( length );
            pw.Write( (byte) 0xAE );
            pw.Write( (short) length );
            pw.Write( 0xFFFFFFFF );
            pw.Write( (ushort) 0xFFFF );
            pw.Write( (byte) 0 );
            pw.Write( (short) 0x03b2 );
            pw.Write( (short) 0x03 );
            pw.WriteAsciiFixed( "ENU\0", 4 );
            pw.WriteAsciiFixed( "System\0", 30 );
            pw.Write( textBytes, 0, textBytes.Length );

            Engine.SendPacketToClient( pw );
        }

        public static void MobileQuery( int serial,
            MobileQueryType queryType = MobileQueryType.StatsRequest )
        {
            PacketWriter pw = new PacketWriter( 10 );
            pw.Write( (byte) 0x34 );
            pw.Write( 0xEDEDEDED );
            pw.Write( (byte) queryType );
            pw.Write( serial );
            Engine.SendPacketToServer( pw );
        }

        public static async Task<int> GetTargeSerialAsync( string message = "", int timeout = 5000 )
        {
            if ( string.IsNullOrEmpty( message ) )
            {
                message = Strings.Target_object___;
            }

            SystemMessage( message );

            Random random = new Random();

            return await Task.Run( () =>
            {
                int value = random.Next( 1, int.MaxValue );

                PacketWriter pw = new PacketWriter( 19 );
                pw.Write( (byte) 0x6C );
                pw.Write( (byte) 0 );
                pw.Write( value );
                pw.Write( (byte) 0 );
                pw.Fill();

                AutoResetEvent are = new AutoResetEvent( false );

                int serial = -1;

                PacketFilterInfo pfi = new PacketFilterInfo( 0x6C,
                    new[] { PacketFilterConditions.IntAtPositionCondition( value, 2 ) },
                    ( packet, info ) =>
                    {
                        serial = ( packet[7] << 24 ) | ( packet[8] << 16 ) | ( packet[9] << 8 ) | packet[10];

                        are.Set();
                    } );

                try
                {
                    Engine.AddSendFilter( pfi );

                    Engine.SendPacketToClient( pw );

                    bool result = are.WaitOne( timeout );

                    if ( result )
                    {
                        return serial;
                    }

                    pw = new PacketWriter( 19 );
                    pw.Write( (byte) 0x6C );
                    pw.Write( (byte) 0 );
                    pw.Write( value );
                    pw.Write( (byte) 3 );
                    pw.Fill();

                    Engine.SendPacketToClient( pw );

                    SystemMessage( Strings.Timeout___ );

                    return serial;
                }
                finally
                {
                    Engine.RemoveSendFilter( pfi );
                }
            } );
        }

        public static void ChangeSkillLock( SkillEntry skill, LockStatus lockStatus )
        {
            byte[] packet = { 0x3A, 0x00, 0x06, 0x00, 0x00, 0x00 };
            packet[4] = (byte) skill.Skill.ID;
            packet[5] = (byte) lockStatus;

            Engine.SendPacketToServer( packet, packet.Length );
        }
    }
}