using ClassicAssist.Resources;

namespace ClassicAssist.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _status = Strings.Ready___;

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MainWindowViewModel()
        {
        }
    }
}