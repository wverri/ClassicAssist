using ClassicAssist.Resources;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class GumpCommands
    {
        public static bool WaitForGump( int gumpId = -1, int timeout = 30000 )
        {
            bool result = UOC.WaitForGump( gumpId, timeout );

            if ( !result )
            {
                UOC.SystemMessage( Strings.Timeout___ );
            }

            return result;
        }

        public static void ReplyGump( int gumpId, int buttonId )
        {
            UOC.GumpButtonClick( gumpId, buttonId );
        }
    }
}