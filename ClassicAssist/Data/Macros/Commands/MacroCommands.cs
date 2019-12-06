using System.Linq;
using System.Threading;
using ClassicAssist.Resources;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class MacroCommands
    {
        public static void PlayMacro( string name )
        {
            MacroManager manager = MacroManager.GetInstance();

            MacroEntry macro = manager.Items.FirstOrDefault( m => m.Name == name );

            if ( macro == null )
            {
                UOC.SystemMessage( Strings.Unknown_macro___ );
                return;
            }

            macro.ActionSync( macro );

            Stop();
        }

        public static void Stop()
        {
            Thread.CurrentThread.Abort();
        }
    }
}