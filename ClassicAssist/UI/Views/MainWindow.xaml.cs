using System.Threading;
using System.Windows;

namespace ClassicAssist.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ResetEvent.Set();
        }

        public ManualResetEvent ResetEvent { get; set; } = new ManualResetEvent( false );
    }
}