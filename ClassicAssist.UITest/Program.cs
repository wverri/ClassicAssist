using System;
using System.Threading;
using System.Windows.Shapes;
using System.Windows.Threading;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.UI.Views;

namespace ClassicAssist.UITest
{
    class Program
    {
        private static Thread _mainThread;
        private static MainWindow _window;

        static void Main(string[] args)
        {
            _mainThread = new Thread(() =>
           {
               _window = new MainWindow();
               _window.ShowDialog();
           })
            { IsBackground = true };

            _mainThread.SetApartmentState(ApartmentState.STA);
            _mainThread.Start();

            _mainThread.Join();
        }
    }
}
