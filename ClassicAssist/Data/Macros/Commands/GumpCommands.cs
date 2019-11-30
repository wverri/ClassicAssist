using System;
using Assistant;
using ClassicAssist.Resources;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class GumpCommands
    {
        [CommandsDisplay(Category = "Gumps", Description = "Pauses until incoming gump packet is received, optional paramters of gump ID and timeout")]
        public static bool WaitForGump( int gumpId = -1, int timeout = 30000 )
        {
            bool result = UOC.WaitForGump( gumpId, timeout );

            if ( !result )
            {
                UOC.SystemMessage( Strings.Timeout___ );
            }

            return result;
        }

        [CommandsDisplay(Category = "Gumps", Description = "Sends a button reply to server gump, parameters are gumpID and buttonID.")]
        public static void ReplyGump( int gumpId, int buttonId )
        {
            UOC.GumpButtonClick( gumpId, buttonId );
        }

        [CommandsDisplay(Category = "Gumps", Description = "Checks if a gump id exists or not.")]
        public static bool GumpExists( int gumpId )
        {
            return Engine.GumpList.ContainsKey( gumpId );
        }

        [CommandsDisplay(Category = "Gumps", Description = "Check for a text in gump.")]
        public static bool InGump( int gumpId, string text )
        {
            //TODO gump parser
            throw new NotImplementedException();
        }
    }
}