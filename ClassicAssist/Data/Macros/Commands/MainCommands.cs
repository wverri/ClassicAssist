using System.Threading;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class MainCommands
    {
        public static void Resync()
        {
            UOC.Resync();
        }

        public static void Pause( int milliseconds )
        {
            Thread.Sleep( milliseconds );
        }
    }
}