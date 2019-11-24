using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassicAssist.Annotations;
using ClassicAssist.UI.Misc;

namespace ClassicAssist.Data.Hotkeys
{
    public class HotkeyManager : INotifyPropertyChanged
    {
        public ObservableCollectionEx<HotkeyEntry> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private static HotkeyManager _instance;
        private ObservableCollectionEx<HotkeyEntry> _items = new ObservableCollectionEx<HotkeyEntry>();

        public HotkeyManager()
        {
            
        }

        public static HotkeyManager GetInstance()
        {
            return _instance ?? ( _instance = new HotkeyManager() );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public void SetProperty<T>(ref T obj, T value, [CallerMemberName] string propertyName = "")
        {
            obj = value;
            OnPropertyChanged(propertyName);
        }
    }
}