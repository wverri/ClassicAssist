using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace ClassicAssist.Data.Macros
{
    public class MacroInvoker
    {
        private readonly MacroEntry _macro;
        private readonly ScriptEngine _engine;
        private ScriptScope _scope;

        public delegate void dMacroStartStop();

        public delegate void dMacroException( Exception e );

        public event dMacroStartStop StartedEvent;
        public event dMacroStartStop StoppedEvent;
        public event dMacroException ExceptionEvent;

        public MacroInvoker( MacroEntry macro )
        {
            _macro = macro;
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();

            ScriptRuntime runtime = _engine.Runtime;
            runtime.LoadAssembly(Assembly.GetExecutingAssembly());
        }

        private static string GetScriptingImports()
        {
            string prepend = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace != null && t.IsPublic && t.IsClass && t.Namespace.EndsWith("Macros.Commands")).Aggregate(string.Empty, (current, t) => current + $"from {t.FullName} import * \n");

            return prepend;
        }

        public bool IsRunning => Thread.IsAlive;
        public bool IsFaulted { get; set; }
        public Exception Exception { get; set; }

        public void Execute()
        {
            string code = $"{GetScriptingImports()}\r\n{_macro.Macro}";

            ScriptSource source = _engine.CreateScriptSourceFromString( code, SourceCodeKind.Statements );

            Thread = new Thread( () =>
            {
                Thread = Thread.CurrentThread;

                try
                {
                    StartedEvent?.Invoke();
                    source.Execute();
                }
                catch ( ThreadAbortException )
                {
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

            while ( !Thread.IsAlive )
            {
                Thread.Sleep( 10 );
            }
        }

        public void Stop()
        {
            Thread?.Abort();
        }

        public Thread Thread { get; set; }
    }
}