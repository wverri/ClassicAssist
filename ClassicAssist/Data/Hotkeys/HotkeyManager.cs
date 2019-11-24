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
        private static readonly object _instanceLock = new object();

        private ObservableCollectionEx<HotkeyEntry> _items = new ObservableCollectionEx<HotkeyEntry>();

        private HotkeyManager()
        {
            
        }

        public static HotkeyManager GetInstance()
        {
            // ReSharper disable once InvertIf
            if ( _instance == null )
            {
                lock ( _instanceLock )
                {
                    if ( _instance == null )
                    {
                        _instance = new HotkeyManager();
                    }
                }
            }

            return _instance;
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