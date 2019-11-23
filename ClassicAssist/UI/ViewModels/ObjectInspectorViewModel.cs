using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using ClassicAssist.Misc;
using ClassicAssist.UI.Models;
using ClassicAssist.UO.Objects;
using Microsoft.Scripting.Utils;

namespace ClassicAssist.UI.ViewModels
{
    public class ObjectInspectorViewModel : BaseViewModel
    {
        private readonly Entity _entity;
        private ObservableCollection<ObjectInspectorData> _items = new ObservableCollection<ObjectInspectorData>();
        private ObjectInspectorData _selectedItem;

        public ObservableCollection<ObjectInspectorData> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ObjectInspectorData SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObjectInspectorViewModel()
        {
            
        }

        public ObjectInspectorViewModel(Entity entity)
        {
            _entity = entity;

            AddPublicProperties( entity );
        }

        private void AddPublicProperties( Entity entity )
        {
            List<ObjectInspectorData> data = new List<ObjectInspectorData>();

            IOrderedEnumerable<PropertyInfo> properties = entity.GetType().GetProperties().OrderBy( ( p ) => p.Name );

            foreach ( PropertyInfo p in properties )
            {
                object value = p.GetValue(entity);
                string valueString = "null";

                if (value != null)
                {
                    valueString = value.ToString();
                }

                DisplayFormatAttribute attr = entity.GetType().GetPropertyAttribute<DisplayFormatAttribute>(p.Name);

                if (attr != null)
                {
                    valueString = attr.ToString(value);
                }

                data.Add(new ObjectInspectorData
                {
                    Name = p.Name,
                    Value = valueString,
                    Category = "Public Properties",
                    IsExpanded = true,
                    OnDoubleClick = null
                });
            }

            Items.AddRange( data );
        }
    }
}