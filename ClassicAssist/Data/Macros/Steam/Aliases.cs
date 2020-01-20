using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.UO.Data;
using UOSteam;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Aliases
    {
        public static void Register()
        {
            Interpreter.RegisterAliasHandler( "backpack", Backpack );
            Interpreter.RegisterAliasHandler( "bank", Bank );
            Interpreter.RegisterAliasHandler( "enemy", Enemy );
            Interpreter.RegisterAliasHandler( "last", Last );
            Interpreter.RegisterAliasHandler( "lasttarget", Last );
            Interpreter.RegisterAliasHandler( "lastobject", LastObject );
            Interpreter.RegisterAliasHandler( "lefthand", LeftHand );
            Interpreter.RegisterAliasHandler( "mount", Mount );
            Interpreter.RegisterAliasHandler( "righthand", RightHand );
            Interpreter.RegisterAliasHandler( "self", Self );
        }

        private static int Self( ref ASTNode node )
        {
            return Engine.Player?.Serial ?? 0;
        }

        private static int RightHand( ref ASTNode node )
        {
            return Engine.Player == null ? 0 : Engine.Player.GetLayer( Layer.TwoHanded );
        }

        private static int Mount( ref ASTNode node )
        {
            return AliasCommands.ResolveSerial( "mount" );
        }

        private static int LeftHand( ref ASTNode node )
        {
            return Engine.Player == null ? 0 : Engine.Player.GetLayer( Layer.OneHanded );
        }

        private static int LastObject( ref ASTNode node )
        {
            return Engine.Player == null ? 0 : Engine.Player.LastObjectSerial;
        }

        private static int Last( ref ASTNode node )
        {
            return AliasCommands.ResolveSerial( "last" );
        }

        private static int Enemy( ref ASTNode node )
        {
            return AliasCommands.ResolveSerial( "enemy" );
        }

        private static int Backpack( ref ASTNode node )
        {
            if ( Engine.Player == null || Engine.Player.Backpack == null )
            {
                return 0;
            }

            return Engine.Player.Backpack.Serial;
        }

        private static int Bank( ref ASTNode node )
        {
            return Engine.Player == null ? 0 : Engine.Player.GetLayer( Layer.Bank );
        }
    }
}