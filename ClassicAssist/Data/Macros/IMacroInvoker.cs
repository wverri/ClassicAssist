namespace ClassicAssist.Data.Macros
{
    public interface IMacroInvoker
    {
        event PythonInvoker.dMacroException ExceptionEvent;
        void Stop();
        void Execute( MacroEntry macro );
    }
}