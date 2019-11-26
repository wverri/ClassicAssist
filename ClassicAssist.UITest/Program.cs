using System;
using System.Threading;
using ClassicAssist.UI.Views;

namespace ClassicAssist.UITest
{
    internal class Program
    {
        private static MainWindow _window;

        [STAThread]
        private static void Main(string[] args)
        {
            _window = new MainWindow();
            _window.ShowDialog();
        }
    }
}