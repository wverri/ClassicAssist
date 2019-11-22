using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using ClassicAssist.Misc;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.UI.ViewModels
{
    public class EntityCollectionViewerViewModel : BaseViewModel
    {
        private ObservableCollection<EntityCollectionData> _entities;
        private bool _showProperties;

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

        public ObservableCollection<EntityCollectionData> Entities
        {
            get => _entities;
            set => SetProperty( ref _entities, value );
        }

        public bool ShowProperties
        {
            get => _showProperties;
            set => SetProperty(ref _showProperties, value);
        }
    }

    public class EntityCollectionData
    {
        public BitmapSource Bitmap => Art.GetStatic( Entity.ID ).ToBitmapSource();
        public Entity Entity { get; set; }
        public string Name => Entity.Name;
    }

    public static class ExtensionMethods
    {
        public static List<EntityCollectionData> ToEntityCollectionData( this ItemCollection itemCollection )
        {
            Item[] items = itemCollection.GetItems();

            return items.Select( item => new EntityCollectionData { Entity = item } ).ToList();
        }
    }
}