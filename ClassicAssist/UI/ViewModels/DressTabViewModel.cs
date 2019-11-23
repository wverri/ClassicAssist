using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Data.Dress;
using ClassicAssist.Data.Hotkeys;
using ClassicAssist.Misc;
using ClassicAssist.UI.Misc;
using ClassicAssist.UO;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using Newtonsoft.Json.Linq;

namespace ClassicAssist.UI.ViewModels
{
    public class DressTabViewModel : BaseViewModel, ISettingProvider
    {
        private readonly Layer[] _validLayers =
        {
            Layer.Arms, Layer.Bracelet, Layer.Cloak, Layer.Earrings, Layer.Gloves, Layer.Helm, Layer.InnerLegs,
            Layer.InnerTorso, Layer.MiddleTorso, Layer.Neck, Layer.OneHanded, Layer.OuterLegs, Layer.OuterTorso,
            Layer.Pants, Layer.Ring, Layer.Shirt, Layer.Shoes, Layer.Talisman, Layer.TwoHanded, Layer.Waist
        };

        private ICommand _dressAllItemsCommand;
        private ICommand _importItemsCommand;
        private bool _isDressingOrUndressing;
        private ObservableCollectionEx<DressAgentEntry> _items = new ObservableCollectionEx<DressAgentEntry>();
        private bool _moveConflictingItems;
        private ICommand _newDressEntryCommand;
        private RelayCommand _removeDressEntryCommand;
        private DressAgentEntry _selectedItem;
        private ICommand _undressAllItemsCommand;
        private bool _useUo3DPackets;

        public DressTabViewModel()
        {
            DressAgentEntry i = new DressAgentEntry { Name = "Dress-1", Items = new List<DressAgentItem>() };

            Items.Add( i );

            SetHotkeyEntries();
        }

        public ICommand DressAllItemsCommand =>
            _dressAllItemsCommand ??
            ( _dressAllItemsCommand =
                new RelayCommandAsync( DressAllItems, o => Engine.Connected && !_isDressingOrUndressing ) );

        public ICommand ImportItemsCommand =>
            _importItemsCommand ?? ( _importItemsCommand = new RelayCommand( ImportItems, o => SelectedItem != null ) );

        public ObservableCollectionEx<DressAgentEntry> Items
        {
            get => _items;
            set => SetProperty( ref _items, value );
        }

        public bool MoveConflictingItems
        {
            get => _moveConflictingItems;
            set => SetProperty( ref _moveConflictingItems, value );
        }

        public ICommand NewDressEntryCommand =>
            _newDressEntryCommand ?? ( _newDressEntryCommand = new RelayCommand( NewDressEntry, o => true ) );

        public ICommand RemoveDressEntryCommand =>
            _removeDressEntryCommand ??
            ( _removeDressEntryCommand = new RelayCommand( RemoveDressEntry, o => SelectedItem != null ) );

        public DressAgentEntry SelectedItem
        {
            get => _selectedItem;
            set => SetProperty( ref _selectedItem, value );
        }

        public ICommand UndressAllItemsCommand =>
            _undressAllItemsCommand ??
            ( _undressAllItemsCommand = new RelayCommandAsync( UndressAllItems, o => Engine.Connected ) );

        public bool UseUO3DPackets
        {
            get => _useUo3DPackets;
            set => SetProperty( ref _useUo3DPackets, value );
        }

        public void Serialize( JObject json )
        {
            JObject dress = new JObject
            {
                {
                    "Options",
                    new JObject { ["MoveConflictingItems"] = MoveConflictingItems, ["UseUO3DPackets"] = UseUO3DPackets }
                }
            };

            JArray dressEntries = new JArray();

            foreach ( DressAgentEntry dae in Items )
            {
                JObject djson = new JObject { ["Name"] = dae.Name, ["UndressContainer"] = dae.UndressContainer };

                JArray items = new JArray();

                if ( dae.Items != null )
                {
                    foreach ( DressAgentItem dressAgentItem in dae.Items )
                    {
                        items.Add( new JObject
                        {
                            ["Layer"] = (int) dressAgentItem.Layer, ["Serial"] = dressAgentItem.Serial
                        } );
                    }
                }

                djson.Add( "Items", items );
                dressEntries.Add( djson );
            }

            dress.Add( "Entries", dressEntries );
            json.Add( "Dress", dress );
        }

        public void Deserialize( JObject json, Options options )
        {
            if ( json["Dress"] == null )
            {
                return;
            }

            JToken dress = json["Dress"];

            MoveConflictingItems = dress["Options"]["MoveConflictingItems"]?.ToObject<bool>() ?? false;
            UseUO3DPackets = dress["Options"]["UseUO3DPackets"]?.ToObject<bool>() ?? false;

            Items.Clear();

            foreach ( JToken entry in dress["Entries"] )
            {
                DressAgentEntry dae = new DressAgentEntry
                {
                    Name = entry["Name"].ToObject<string>(),
                    UndressContainer = entry["UndressContainer"].ToObject<int>()
                };

                if ( entry["Items"] != null )
                {
                    List<DressAgentItem> items = new List<DressAgentItem>();

                    foreach ( JToken itemEntry in entry["Items"] )
                    {
                        items.Add( new DressAgentItem
                        {
                            Layer = itemEntry["Layer"].ToObject<Layer>(),
                            Serial = itemEntry["Serial"].ToObject<int>()
                        } );

                        dae.Items = items.ToArray();
                    }
                }

                Items.Add( dae );
            }
        }

        private async Task UndressAllItems( object obj )
        {
            PlayerMobile player = Engine.Player;

            if ( player == null )
            {
                return;
            }

            int backpack = player.Backpack;

            if ( backpack <= 0 )
            {
                return;
            }

            int[] items = player.GetEquippedItems().Where( i => IsValidLayer( i.Layer ) ).Select( i => i.Serial )
                .ToArray();

            foreach ( int item in items )
            {
                await Commands.DragDropAsync( item, 1, backpack );
            }
        }

        private void SetHotkeyEntries()
        {
            HotkeyEntry category = new HotkeyEntry { Name = "Dress", IsCategory = true };

            //_hotkeys.Items.Add(category);

            Items.CollectionChanged += ( sender, args ) => { SetHotkeyChildren( category ); };
            SetHotkeyChildren( category );
        }

        private void SetHotkeyChildren( HotkeyEntry category )
        {
            ObservableCollectionEx<HotkeySettable> list = new ObservableCollectionEx<HotkeySettable>();

            foreach ( DressAgentEntry item in Items )
            {
                list.Add( item );
            }

            category.Children = list;
        }

        private void NewDressEntry( object obj )
        {
            int count = Items.Count;

            Items.Add( new DressAgentEntry { Name = $"Dress-{count + 1}", Items = new List<DressAgentItem>() } );
        }

        private void RemoveDressEntry( object obj )
        {
            if ( !( obj is DressAgentEntry dae ) )
            {
                return;
            }

            dae.Hotkey = ShortcutKeys.Default;
            Items.Remove( dae );
        }

        private async Task DressAllItems( object obj )
        {
            if ( !( obj is DressAgentEntry dae ) )
            {
                return;
            }

            PlayerMobile player = Engine.Player;

            if ( player == null )
            {
                return;
            }

            await Task.Run( async () =>
            {
                try
                {
                    _isDressingOrUndressing = true;

                    foreach ( DressAgentItem dai in dae.Items )
                    {
                        Item item = Engine.Items.GetItem( dai.Serial );

                        if ( item == null )
                        {
                            continue;
                        }

                        Commands.EquipItem( item, dai.Layer );

                        await Task.Delay( 800 );
                    }
                }
                finally
                {
                    _isDressingOrUndressing = false;
                }
            } );
        }

        private void ImportItems( object obj )
        {
            if ( !( obj is DressAgentEntry dae ) )
            {
                return;
            }

            PlayerMobile player = Engine.Player;

            List<DressAgentItem> items = player.GetEquippedItems().Where( i => IsValidLayer( i.Layer ) )
                .Select( i => new DressAgentItem { Serial = i.Serial, Layer = i.Layer } ).ToList();

            dae.Items = items;
        }

        public bool IsValidLayer( Layer layer )
        {
            return _validLayers.Any( l => l == layer );
        }
    }
}