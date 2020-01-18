using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Data.Autoloot;
using ClassicAssist.Misc;
using ClassicAssist.Resources;
using ClassicAssist.UI.Misc;
using ClassicAssist.UI.Views;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.UI.ViewModels.Agents
{
    public class AutolootViewModel : BaseViewModel, ISettingProvider
    {
        private ObservableCollection<AutolootConstraints>
            _constraints = new ObservableCollection<AutolootConstraints>();

        private int _containerSerial;

        private bool _disableInGuardzone;
        private bool _enabled;

        private ICommand _insertCommand;
        private ICommand _insertConstraintCommand;

        private ObservableCollectionEx<AutolootEntry> _items = new ObservableCollectionEx<AutolootEntry>();
        private ICommand _removeCommand;
        private ICommand _removeConstraintCommand;
        private AutolootEntry _selectedItem;
        private ICommand _selectHueCommand;
        private ICommand _setContainerCommand;

        public AutolootViewModel()
        {
            string constraintsFile = Path.Combine( Engine.StartupPath ?? Environment.CurrentDirectory, "Data",
                "Autoloot.json" );

            if ( !File.Exists( constraintsFile ) )
            {
                return;
            }

            JsonSerializer serializer = new JsonSerializer();

            using ( StreamReader sr = new StreamReader( constraintsFile ) )
            {
                using ( JsonTextReader reader = new JsonTextReader( sr ) )
                {
                    AutolootConstraints[] constraints = serializer.Deserialize<AutolootConstraints[]>( reader );

                    foreach ( AutolootConstraints constraint in constraints )
                    {
                        Constraints.Add( constraint );
                    }
                }
            }
        }

        public ObservableCollection<AutolootConstraints> Constraints
        {
            get => _constraints;
            set => SetProperty( ref _constraints, value );
        }

        public int ContainerSerial
        {
            get => _containerSerial;
            set => SetProperty( ref _containerSerial, value );
        }

        public bool DisableInGuardzone
        {
            get => _disableInGuardzone;
            set => SetProperty( ref _disableInGuardzone, value );
        }

        public bool Enabled
        {
            get => _enabled;
            set => SetProperty( ref _enabled, value );
        }

        public ICommand InsertCommand =>
            _insertCommand ?? ( _insertCommand = new RelayCommandAsync( Insert, o => Engine.Connected ) );

        public ICommand InsertConstraintCommand =>
            _insertConstraintCommand ?? ( _insertConstraintCommand =
                new RelayCommand( InsertConstraint, o => SelectedItem != null ) );

        public ObservableCollectionEx<AutolootEntry> Items
        {
            get => _items;
            set => SetProperty( ref _items, value );
        }

        public ICommand RemoveCommand =>
            _removeCommand ?? ( _removeCommand = new RelayCommandAsync( Remove, o => SelectedItem != null ) );

        public ICommand RemoveConstraintCommand =>
            _removeConstraintCommand ?? ( _removeConstraintCommand =
                new RelayCommand( RemoveConstraint, o => SelectedConstraint != null ) );

        public AutolootConstraints SelectedConstraint { get; set; }

        public AutolootEntry SelectedItem
        {
            get => _selectedItem;
            set => SetProperty( ref _selectedItem, value );
        }

        public ICommand SelectHueCommand =>
            _selectHueCommand ?? ( _selectHueCommand = new RelayCommand( SelectHue, o => SelectedItem != null ) );

        public ICommand SetContainerCommand =>
            _setContainerCommand ?? ( _setContainerCommand = new RelayCommandAsync( SetContainer, o => true ) );

        public void Serialize( JObject json )
        {
            if ( json == null )
            {
                return;
            }

            JObject autolootObj = new JObject
            {
                { "Enabled", Enabled },
                { "DisableInGuardzone", DisableInGuardzone },
                { "Container", ContainerSerial }
            };

            JArray itemsArray = new JArray();

            foreach ( AutolootEntry entry in Items )
            {
                JObject entryObj = new JObject
                {
                    { "Name", entry.Name },
                    { "ID", entry.ID },
                    { "Autoloot", entry.Autoloot },
                    { "Rehue", entry.Rehue },
                    { "RehueHue", entry.RehueHue }
                };

                if ( entry.Constraints != null )
                {
                    JArray constraintsArray = new JArray();

                    foreach ( AutolootConstraints constraint in entry.Constraints )
                    {
                        JObject constraintObj = new JObject
                        {
                            { "Name", constraint.Name },
                            { "Cliloc", constraint.Cliloc },
                            { "ConstraintType", constraint.ConstraintType.ToString() },
                            { "Operator", constraint.Operator },
                            { "Value", constraint.Value }
                        };

                        constraintsArray.Add( constraintObj );
                    }

                    entryObj.Add( "Constraints", constraintsArray );
                }

                itemsArray.Add( entryObj );
            }

            autolootObj.Add( "Items", itemsArray );

            json.Add( "Autoloot", autolootObj );
        }

        public void Deserialize( JObject json, Options options )
        {
            if ( json?["Autoloot"] == null )
            {
                return;
            }

            JToken config = json["Autoloot"];

            Enabled = config["Enabled"]?.ToObject<bool>() ?? true;
            DisableInGuardzone = config["DisableInGuardzone"]?.ToObject<bool>() ?? false;
            ContainerSerial = config["Container"]?.ToObject<int>() ?? 0;

            if ( config["Items"] != null )
            {
                JToken items = config["Items"];

                foreach ( JToken token in items )
                {
                    AutolootEntry entry = new AutolootEntry
                    {
                        Name = token["Name"]?.ToObject<string>() ?? "Unknown",
                        ID = token["ID"]?.ToObject<int>() ?? 0,
                        Autoloot = token["Autoloot"]?.ToObject<bool>() ?? false,
                        Rehue = token["Rehue"]?.ToObject<bool>() ?? false,
                        RehueHue = token["RehueHue"]?.ToObject<int>() ?? 0
                    };

                    if ( token["Constraints"] != null )
                    {
                        List<AutolootConstraints> constraintsList = new List<AutolootConstraints>();

                        foreach ( JToken constraintToken in token["Constraints"] )
                        {
                            AutolootConstraints constraintObj = new AutolootConstraints
                            {
                                Name = constraintToken["Name"]?.ToObject<string>() ?? "Unknown",
                                Cliloc = constraintToken["Cliloc"]?.ToObject<int>() ?? 0,
                                ConstraintType =
                                    constraintToken["ConstraintType"]?.ToObject<AutolootConstraintType>() ??
                                    AutolootConstraintType.Object,
                                Operator = constraintToken["Operator"]?.ToObject<string>() ?? "==",
                                Value = constraintToken["Value"]?.ToObject<int>() ?? 0
                            };

                            constraintsList.Add( constraintObj );
                        }

                        entry.Constraints.AddRange( constraintsList );
                    }

                    Items.Add( entry );
                }
            }
        }

        private void RemoveConstraint( object obj )
        {
            if ( !( obj is AutolootConstraints constraint ) )
            {
                return;
            }

            SelectedItem.Constraints.Remove( constraint );
        }

        private void InsertConstraint( object obj )
        {
            if ( !( obj is AutolootConstraints constraint ) )
            {
                return;
            }

            List<AutolootConstraints> constraints =
                new List<AutolootConstraints>( SelectedItem.Constraints ) { constraint };

            SelectedItem.Constraints = new ObservableCollection<AutolootConstraints>( constraints );
        }

        private async Task SetContainer( object arg )
        {
            int serial = await UOC.GetTargeSerialAsync( Strings.Target_container___ );

            if ( serial == 0 )
            {
                UOC.SystemMessage( Strings.Invalid_or_unknown_object_id );
                return;
            }

            ContainerSerial = serial;
        }

        private async Task Insert( object arg )
        {
            int serial = await UOC.GetTargeSerialAsync( Strings.Target_object___ );

            if ( serial == 0 )
            {
                UOC.SystemMessage( Strings.Invalid_or_unknown_object_id );
                return;
            }

            Item item = Engine.Items.GetItem( serial );

            if ( item == null )
            {
                UOC.SystemMessage( Strings.Cannot_find_item___ );
                return;
            }

            Items.Add( new AutolootEntry
            {
                Name = TileData.GetStaticTile( item.ID ).Name, ID = item.ID, Constraints = null
            } );
        }

        private async Task Remove( object arg )
        {
            if ( !( arg is AutolootEntry entry ) )
            {
                return;
            }

            Items.Remove( entry );

            await Task.CompletedTask;
        }

        private static void SelectHue( object obj )
        {
            if ( !( obj is AutolootEntry entry ) )
            {
                return;
            }

            if ( HuePickerWindow.GetHue( out int hue ) )
            {
                entry.RehueHue = hue;
            }
        }
    }
}