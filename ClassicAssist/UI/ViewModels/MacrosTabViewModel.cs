using System.Windows.Input;
using ClassicAssist.Data;
using ClassicAssist.Data.Hotkeys;
using ClassicAssist.Data.Macros;
using ClassicAssist.Misc;
using ClassicAssist.UI.Misc;
using Newtonsoft.Json.Linq;

namespace ClassicAssist.UI.ViewModels
{
    public class MacrosTabViewModel : BaseViewModel, ISettingProvider
    {
        private HotkeyManager _hotkeys;
        private bool _isPlaying;
        private ObservableCollectionEx<MacroEntry> _items = new ObservableCollectionEx<MacroEntry>();
        private ICommand _newMacroCommand;
        private RelayCommand _removeMacroCommand;
        private MacroEntry _selectedItem;

        public MacrosTabViewModel()
        {
            _hotkeys = HotkeyManager.GetInstance();
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty( ref _isPlaying, value );
        }

        public ObservableCollectionEx<MacroEntry> Items
        {
            get => _items;
            set => SetProperty( ref _items, value );
        }

        public ICommand NewMacroCommand =>
            _newMacroCommand ?? ( _newMacroCommand = new RelayCommand( NewMacro, o => !IsPlaying ) );

        public ICommand RemoveMacroCommand =>
            _removeMacroCommand ?? ( _removeMacroCommand =
                new RelayCommand( RemoveMacro, o => !IsPlaying && SelectedItem != null ) );

        public MacroEntry SelectedItem
        {
            get => _selectedItem;
            set => SetProperty( ref _selectedItem, value );
        }

        public void Serialize( JObject json )
        {
        }

        public void Deserialize( JObject json, Options options )
        {
        }

        private void NewMacro( object obj )
        {
            int count = Items.Count;

            Items.Add( new MacroEntry { Name = $"Macro-{count + 1}", Macro = string.Empty } );
        }

        private void RemoveMacro( object obj )
        {
            if ( obj is MacroEntry entry )
            {
                Items.Remove( entry );
            }
        }
    }
}