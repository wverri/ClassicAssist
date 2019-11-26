using Assistant;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class MobileCommands
    {
        public static int Hits( object mobile = null )
        {
            if ( mobile == null )
            {
                return Engine.Player?.Hits ?? 0;
            }

            return 0;
        }
    }
}