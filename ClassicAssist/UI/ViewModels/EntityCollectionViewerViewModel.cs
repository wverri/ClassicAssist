using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Assistant;
using ClassicAssist.Misc;
using ClassicAssist.UI.Views;
using ClassicAssist.UO;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.UI.ViewModels
{
    public class EntityCollectionViewerViewModel : BaseViewModel
    {
        private ICommand _cancelActionCommand;
        private CancellationTokenSource _cancellationToken;
        private ICommand _contextMoveToBackpackCommand;
        private ObservableCollection<EntityCollectionData> _entities;
        private bool _isPerformingAction;
        private ICommand _itemDoubleClickCommand;

        private ObservableCollection<EntityCollectionData> _selectedItems =
            new ObservableCollection<EntityCollectionData>();

        private bool _showProperties;
        private ICommand _togglePropertiesCommand;
        private bool _topmost;

        public EntityCollectionViewerViewModel()
        {
            Entities = new ObservableCollection<EntityCollectionData>
            {
                new EntityCollectionData { Entity = new Item( 1 ) },
                new EntityCollectionData { Entity = new Item( 2 ) }
            };
        }

        public EntityCollectionViewerViewModel( ItemCollection collection )
        {
            Entities = new ObservableCollection<EntityCollectionData>( collection.ToEntityCollectionData() );
        }

        public ICommand CancelActionCommand =>
            _cancelActionCommand ?? ( _cancelActionCommand = new RelayCommandAsync( CancelAction,
                o => _cancellationToken != null ) );

        public ICommand ContextMoveToBackpackCommand =>
            _contextMoveToBackpackCommand ?? ( _contextMoveToBackpackCommand =
                new RelayCommandAsync( ContextMoveToBackpack,
                    o => SelectedItems != null || SelectedItems?.Count != 0 ) );

        public ObservableCollection<EntityCollectionData> Entities
        {
            get => _entities;
            set => SetProperty( ref _entities, value );
        }

        public bool IsPerformingAction
        {
            get => _isPerformingAction;
            set => SetProperty( ref _isPerformingAction, value );
        }

        public ICommand ItemDoubleClickCommand =>
            _itemDoubleClickCommand ?? ( _itemDoubleClickCommand = new RelayCommand( ItemDoubleClick, o => true ) );

        public ObservableCollection<EntityCollectionData> SelectedItems
        {
            get => _selectedItems;
            set => SetProperty( ref _selectedItems, value );
        }

        public bool ShowProperties
        {
            get => _showProperties;
            set => SetProperty( ref _showProperties, value );
        }

        public ICommand TogglePropertiesCommand =>
            _togglePropertiesCommand ?? ( _togglePropertiesCommand = new RelayCommand( ToggleProperties, o => true ) );

        public bool Topmost
        {
            get => _topmost;
            set => SetProperty( ref _topmost, value );
        }

        private async Task ContextMoveToBackpack( object arg )
        {
            _cancellationToken = new CancellationTokenSource();

            if ( Engine.Player == null || Engine.Player.Backpack == null )
            {
                return;
            }

            try
            {
                IsPerformingAction = true;

                int[] items = SelectedItems.Select( i => i.Entity.Serial ).ToArray();

                foreach ( int item in items )
                {
                    await Commands.DragDropAsync( item, -1, Engine.Player.Backpack.Serial );

                    if ( _cancellationToken.Token.IsCancellationRequested )
                    {
                        return;
                    }
                }
            }
            finally
            {
                IsPerformingAction = false;
            }
        }

        private async Task CancelAction( object arg )
        {
            _cancellationToken?.Cancel();

            await Task.CompletedTask;
        }

        private void ToggleProperties( object obj )
        {
            if ( !( obj is bool showProperties ) )
            {
                return;
            }

            ShowProperties = showProperties;
        }

        private static void ItemDoubleClick( object obj )
        {
            if ( !( obj is EntityCollectionData ecd ) )
            {
                return;
            }

            ObjectInspectorWindow window =
                new ObjectInspectorWindow { DataContext = new ObjectInspectorViewModel( ecd.Entity ) };

            window.ShowDialog();
        }
    }

    public class EntityCollectionData
    {
        public BitmapSource Bitmap => Art.GetStatic( Entity.ID, Entity.Hue ).ToBitmapSource();
        public Entity Entity { get; set; }
        public string FullName => GetProperties( Entity );
        public string Name => Entity.Name;

        private static string GetProperties( Entity entity )
        {
            return entity.Properties == null
                ? entity.Name
                : entity.Properties.Aggregate( "",
                    ( current, entityProperty ) => current + entityProperty.Text + "\r\n" );
        }
    }

    public static class ExtensionMethods
    {
        public static List<EntityCollectionData> ToEntityCollectionData( this ItemCollection itemCollection )
        {
            if ( itemCollection == null )
            {
                return new List<EntityCollectionData>();
            }

            Item[] items = itemCollection.GetItems();

            IEnumerable<Item> noNames = items.Where( i => string.IsNullOrEmpty( i.Name ) );

            foreach ( Item item in noNames )
            {
                item.Name = $"0x{item.Serial:x8}";
            }

            return items.Select( item => new EntityCollectionData { Entity = item } ).ToList();
        }
    }
}