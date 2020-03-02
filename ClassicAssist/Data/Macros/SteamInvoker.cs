using System;
using System.Threading;
using System.Threading.Tasks;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Data.Macros.Steam;
using UOScript;

namespace ClassicAssist.Data.Macros
{
    public class SteamInvoker : IMacroInvoker
    {
        private static SteamInvoker _instance;
        private static readonly object _lock = new object();
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private MacroEntry _macro;

        public SteamInvoker()
        {
            Aliases.Register();
            Steam.Commands.Register();
            Expressions.Register();
        }

        public Exception Exception { get; set; }

        public bool IsFaulted { get; set; }

        public event PythonInvoker.dMacroException ExceptionEvent;
        public event PythonInvoker.dMacroStartStop StoppedEvent;

        public bool IsRunning
        {
            get => Thread?.IsAlive ?? false;
            set => throw new InvalidOperationException();
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
            Thread.Interrupt();
            Thread.Abort();
        }

        public void Execute( MacroEntry macro )
        {
            _macro = macro;

            if ( Thread != null && Thread.IsAlive )
            {
                Stop();
            }

            MainCommands.SetQuietMode( Options.CurrentOptions.DefaultMacroQuietMode );

            _cancellationToken = new CancellationTokenSource();

            IsFaulted = false;

            Thread = new Thread( () =>
            {
                Thread = Thread.CurrentThread;

                try
                {
                    do
                    {
                        Interpreter.StopScript();

                        StartedEvent?.Invoke();

                        AliasCommands.SetDefaultAliases();

                        ASTNode ast = Lexer.Lex( macro.Macro.Split( '\n' ) );

                        Script script = new Script( ast );

                        Interpreter.StartScript( script );

                        while ( Interpreter.ExecuteScript() )
                        {
                            _cancellationToken.Token.ThrowIfCancellationRequested();
                        }
                    }
                    while ( _macro.Loop && !IsFaulted );
                }
                catch ( TaskCanceledException )
                {
                    IsFaulted = true;
                }
                catch ( ThreadInterruptedException )
                {
                    IsFaulted = true;
                }
                catch ( ThreadAbortException )
                {
                    IsFaulted = true;
                }
                catch ( Exception e )
                {
                    IsFaulted = true;
                    Exception = e;

                    ExceptionEvent?.Invoke( e );
                }
                finally
                {
                    StoppedEvent?.Invoke();
                }
            } ) { IsBackground = true };

            Thread.Start();
        }

        public Thread Thread { get; set; }
        public event PythonInvoker.dMacroStartStop StartedEvent;

        public static SteamInvoker GetInstance()
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

                    _instance = new SteamInvoker();
                    return _instance;
                }
            }

            return _instance;
        }
    }
}