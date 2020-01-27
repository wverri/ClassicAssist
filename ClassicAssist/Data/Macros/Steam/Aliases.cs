using Assistant;
using ClassicAssist.Data.Macros.Commands;
using UOSteam;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Aliases
    {
        public static void Register()
        {
            Interpreter.RegisterAliasHandler( "backpack", Backpack );
            //Interpreter.RegisterAliasHandler( "bank", Bank );
            //Interpreter.RegisterAliasHandler( "enemy", Enemy );
            Interpreter.RegisterAliasHandler( "last", Last );
            //Interpreter.RegisterAliasHandler( "lasttarget", Last );
            //Interpreter.RegisterAliasHandler( "lastobject", LastObject );
            //Interpreter.RegisterAliasHandler( "lefthand", LeftHand );
            //Interpreter.RegisterAliasHandler( "mount", Mount );
            //Interpreter.RegisterAliasHandler( "righthand", RightHand );
            Interpreter.RegisterAliasHandler( "self", Self );
        }

        private static uint Last( string alias )
        {
            return (uint) AliasCommands.GetAlias( "last" );
        }

        private static uint Backpack( string alias )
        {
            if ( Engine.Player == null || Engine.Player.Backpack == null )
            {
                return 0;
            }

            return (uint) Engine.Player.Backpack.Serial;
        }

        private static uint Self( string alias )
        {
            return (uint) ( Engine.Player?.Serial ?? 0 );
        }
    }
}