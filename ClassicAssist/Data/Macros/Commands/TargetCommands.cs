using Assistant;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network.Packets;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class TargetCommands
    {   
        public static bool WaitForTarget( int timeout = 5000 )
        {
            return UOC.WaitForTarget( timeout );
        }

        public static void Target( object obj )
        {
            int serial = AliasCommands.ResolveSerial( obj );

            if ( serial <= 0 )
                return;

            Engine.SendPacketToServer( new Target( TargetType.Object, -1, TargetFlags.None, serial, -1, -1, -1, 0 ) );
            Engine.SendPacketToClient( new Target( TargetType.Object, -1, TargetFlags.Cancel, -1, -1, -1,
                0, 0 ) );

        }
    }
}