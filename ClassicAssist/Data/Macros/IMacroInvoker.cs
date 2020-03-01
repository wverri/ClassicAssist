using System;
using System.Threading;

namespace ClassicAssist.Data.Macros
{
    public interface IMacroInvoker
    {
        event PythonInvoker.dMacroException ExceptionEvent;
        event PythonInvoker.dMacroStartStop StoppedEvent;
        void Stop();
        void Execute( MacroEntry macro );
        bool IsRunning { get; set; }
        Thread Thread { get; set; }
    }
}