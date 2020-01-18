using System;
using System.Collections.Generic;
using UOSteam;

namespace ClassicAssist.Data.Macros
{
    public class SteamEngineInvoker : IMacroInvoker
    {
        private static SteamEngineInvoker _instance;
        private static readonly object _lock = new object();

        public event PythonInvoker.dMacroException ExceptionEvent;

        public void Stop()
        {
        }

        public void Execute( MacroEntry macro )
        {
            ASTNode ast = Lexer.Lex( macro.Macro.Split( '\n' ) );

            Script script = new Script( ast );

            Interpreter.RegisterCommandHandler( "sysmsg", ParseCommand );

            try
            {
                Interpreter.StartScript( script );

                while ( Interpreter.ExecuteScripts() )
                {
                }
            }
            catch ( SyntaxError se )
            {
                UO.Commands.SystemMessage( se.ToString() );
            }
            catch ( RunTimeError re )
            {
                UO.Commands.SystemMessage( re.ToString() );
            }
        }

        public static SteamEngineInvoker GetInstance()
        {
            // ReSharper disable once InvertIf
            if ( _instance == null )
            {
                lock ( _lock )
                {
                    if ( _instance != null )
                    {
                        return _instance;
                    }

                    _instance = new SteamEngineInvoker();
                    return _instance;
                }
            }

            return _instance;
        }

        private static bool ParseCommand( ref ASTNode node, bool quiet, bool force )
        {
            object[] param = ParseParams( node );

            string text = GetParam( param, 0, string.Empty );
            int hue = GetParam( param, 1, 946 );

            UO.Commands.SystemMessage( text, hue );

            node = null;

            return true;
        }

        private static T GetParam<T>( IReadOnlyList<object> objects, int i, T defaultValue )
        {
            try
            {
                return (T) objects[i];
            }
            catch ( Exception )
            {
                return defaultValue;
            }
        }

        private static object[] ParseParams( ASTNode node )
        {
            List<object> paramList = new List<object>();

            ASTNode nextNode = node.Next();

            do
            {
                if ( nextNode == null )
                {
                    break;
                }

                switch ( nextNode.Type )
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
                        paramList.Add( nextNode.Lexeme );
                        break;
                    case ASTNodeType.SERIAL:
                        paramList.Add( Convert.ToInt32( nextNode.Lexeme ) );
                        break;
                    case ASTNodeType.INTEGER:
                        paramList.Add( Convert.ToInt32( nextNode.Lexeme ) );
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

                nextNode = nextNode.Next();
            }
            while ( nextNode != null );

            return paramList.ToArray();
        }
    }
}