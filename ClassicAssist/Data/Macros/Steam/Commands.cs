using System;
using System.Collections.Generic;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Resources;
using UOSteam;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Commands
    {
        public static void Register()
        {
            Interpreter.RegisterCommandHandler( "fly", Fly );
            Interpreter.RegisterCommandHandler( "land", Land );
            Interpreter.RegisterCommandHandler( "setability", SetAbility );
            Interpreter.RegisterCommandHandler( "attack", Attack );
            Interpreter.RegisterCommandHandler( "sysmsg", SysMsg );
            Interpreter.RegisterCommandHandler( "useskill", UseSkill );
            Interpreter.RegisterCommandHandler( "clearhands", ClearHands );
            Interpreter.RegisterCommandHandler( "clickobject", ClickObject );
            Interpreter.RegisterCommandHandler( "bandageself", BandageSelf );
            Interpreter.RegisterCommandHandler( "usetype", UseType );
            Interpreter.RegisterCommandHandler( "useobject", UseObject );
        }

        private static bool UseObject( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: useobject 'alias'/serial" );
                node = null;
                return true;
            }

            object obj = GetArgument<object>( args, 0, null, true );

            ObjectCommands.UseObject( obj );

            node = null;

            return true;
        }

        private static bool UseType( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: usetype 'alias'/id [hue] [container]" );
                node = null;
                return true;
            }

            object obj = GetArgument<object>( args, 0, null, true );
            int hue = GetArgument( args, 1, -1 );
            object container = GetArgument<object>( args, 2, null );

            ObjectCommands.UseType( obj, hue, container );

            node = null;

            return true;
        }

        // ReSharper disable once RedundantAssignment
        private static bool BandageSelf( ref ASTNode node, bool quiet, bool force )
        {
            ConsumeCommands.BandageSelf();

            node = null;

            return true;
        }

        private static bool ClickObject( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: clickobject 'alias'/serial" );
                node = null;
                return true;
            }

            object obj = GetArgument<object>( args, 0, null, true );

            ActionCommands.ClickObject( obj );

            node = null;

            return true;
        }

        private static bool ClearHands( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: clearhands 'left/right/both'" );
                node = null;
                return true;
            }

            string hands = GetArgument( args, 0, string.Empty, true );

            ActionCommands.ClearHands( hands );

            node = null;

            return true;
        }

        private static bool Attack( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: attack 'alias'/serial" );
                node = null;
                return true;
            }

            object obj = GetArgument<object>( args, 0, null, true );

            ActionCommands.Attack( obj );

            node = null;

            return true;
        }

        private static bool SetAbility( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: setability 'primary/secondary' ['on'/'off'/'toggle']" );
                node = null;
                return true;
            }

            string ability = GetArgument( args, 0, string.Empty, true );
            string onOff = GetArgument( args, 1, "toggle" );

            AbilitiesCommands.SetAbility( ability, onOff );

            node = null;
            return true;
        }

        // ReSharper disable once RedundantAssignment
        private static bool Land( ref ASTNode node, bool quiet, bool force )
        {
            AbilitiesCommands.Land();

            node = null;
            return true;
        }

        // ReSharper disable once RedundantAssignment
        private static bool Fly( ref ASTNode node, bool quiet, bool force )
        {
            AbilitiesCommands.Fly();

            node = null;
            return true;
        }

        private static bool UseSkill( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: useskill 'skill/last'" );
                node = null;
                return true;
            }

            string skill = GetArgument( args, 0, string.Empty, true );

            SkillCommands.UseSkill( skill );

            node = null;
            return true;
        }

        private static bool SysMsg( ref ASTNode node, bool quiet, bool force )
        {
            node = node.Next();

            object[] args = ParseArguments( node );

            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: sysmsg 'text' [hue]" );
                node = null;
                return true;
            }

            string text = GetArgument( args, 0, string.Empty );
            int hue = GetArgument( args, 1, 946 );

            UOC.SystemMessage( text, hue );

            node = null;

            return true;
        }

        private static T GetArgument<T>( IReadOnlyList<object> objects, int i, T defaultValue,
            bool throwException = false )
        {
            try
            {
                return (T) objects[i];
            }
            catch ( Exception )
            {
                if ( throwException )
                {
                    throw new ArgumentException( Strings.Required_argument_was_missing_ );
                }

                return defaultValue;
            }
        }

        private static object[] ParseArguments( ASTNode node )
        {
            List<object> argsList = new List<object>();

            do
            {
                if ( node == null )
                {
                    break;
                }

                switch ( node.Type )
                {
                    case ASTNodeType.IF:
                        break;
                    case ASTNodeType.ELSEIF:
                        break;
                    case ASTNodeType.ELSE:
                        break;
                    case ASTNodeType.ENDIF:
                        break;
                    case ASTNodeType.WHILE:
                        break;
                    case ASTNodeType.ENDWHILE:
                        break;
                    case ASTNodeType.FOR:
                        break;
                    case ASTNodeType.ENDFOR:
                        break;
                    case ASTNodeType.BREAK:
                        break;
                    case ASTNodeType.CONTINUE:
                        break;
                    case ASTNodeType.STOP:
                        break;
                    case ASTNodeType.REPLAY:
                        break;
                    case ASTNodeType.EQUAL:
                        break;
                    case ASTNodeType.NOT_EQUAL:
                        break;
                    case ASTNodeType.LESS_THAN:
                        break;
                    case ASTNodeType.LESS_THAN_OR_EQUAL:
                        break;
                    case ASTNodeType.GREATER_THAN:
                        break;
                    case ASTNodeType.GREATER_THAN_OR_EQUAL:
                        break;
                    case ASTNodeType.NOT:
                        break;
                    case ASTNodeType.AND:
                        break;
                    case ASTNodeType.OR:
                        break;
                    case ASTNodeType.STRING:
                        argsList.Add( node.Lexeme );
                        break;
                    case ASTNodeType.SERIAL:
                        argsList.Add( Convert.ToInt32( node.Lexeme, 16 ) );
                        break;
                    case ASTNodeType.INTEGER:
                        argsList.Add( Convert.ToInt32( node.Lexeme ) );
                        break;
                    case ASTNodeType.QUIET:
                        break;
                    case ASTNodeType.FORCE:
                        break;
                    case ASTNodeType.SCRIPT:
                        break;
                    case ASTNodeType.STATEMENT:
                        break;
                    case ASTNodeType.COMMAND:
                        break;
                    case ASTNodeType.LOGICAL_EXPRESSION:
                        break;
                    case ASTNodeType.UNARY_EXPRESSION:
                        break;
                    case ASTNodeType.BINARY_EXPRESSION:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                node = node.Next();
            }
            while ( node != null );

            return argsList.ToArray();
        }
    }
}