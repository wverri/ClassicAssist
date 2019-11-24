using System.Windows.Input;
using ClassicAssist.Data.Hotkeys;
using ClassicAssist.UI.Misc;

namespace ClassicAssist.UI.ViewModels
{
    public class HotkeysTabViewModel : BaseViewModel
    {
        private ICommand _clearHotkeyCommand;
        private ICommand _executeCommand;
        private ObservableCollectionEx<HotkeyEntry> _items = new ObservableCollectionEx<HotkeyEntry>();
        private HotkeySettable _selectedItem;
        private readonly HotkeyManager _hotkeyManager;

        public ICommand ClearHotkeyCommand =>
            _clearHotkeyCommand ?? ( _clearHotkeyCommand = new RelayCommand( ClearHotkey, o => SelectedItem != null ) );

        public ICommand ExecuteCommand =>
            _executeCommand ?? ( _executeCommand = new RelayCommand( ExecuteHotkey, o => SelectedItem != null ) );

        public ObservableCollectionEx<HotkeyEntry> Items
        {
            get => _hotkeyManager.Items;
            set => _hotkeyManager.Items = value;
        }

        public HotkeysTabViewModel()
        {
            _hotkeyManager = HotkeyManager.GetInstance();
        }

        public HotkeySettable SelectedItem
        {
            get => _selectedItem;
            set => SetProperty( ref _selectedItem, value );
        }

        private static void ClearHotkey( object obj )
        {
            if ( obj is HotkeySettable cmd )
            {
                cmd.Hotkey = ShortcutKeys.Default;
            }
        }

        private static void ExecuteHotkey( object obj )
        {
            if ( obj is HotkeySettable cmd )
            {
                cmd.Action( cmd );
            }
        }
    }
}