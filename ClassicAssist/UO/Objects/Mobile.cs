using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assistant;
using ClassicAssist.UO.Data;

namespace ClassicAssist.UO.Objects
{
    public class Mobile : Entity
    {
        internal int[] _layerArray = new int[(int)Layer.LastValid + 1];
        public ItemCollection Equipment { get; set; }

        public Mobile( int serial ) : base( serial )
        {
            Equipment = new ItemCollection( serial );
        }

        public int Hits { get; set; }
        public int HitsMax { get; set; }
        public Notoriety Notoriety { get; set; }
        public MobileStatus Status { get; set; }

        internal void SetLayer(Layer layer, int serial)
        {
            if ((int)layer >= _layerArray.Length)
            {
                return;
            }

            Interlocked.Exchange(ref _layerArray[(int)layer], serial);
        }

        internal int[] GetAllLayers()
        {
            return _layerArray;
        }

        internal int GetLayer(Layer layer)
        {
            return (int)layer >= _layerArray.Length ? 0 : Thread.VolatileRead(ref _layerArray[(int)layer]);
        }

        public Item[] GetEquippedItems()
        {
            List<Item> itemList = new List<Item>();

            int[] layerArray = GetAllLayers();

            IEnumerable<int> layers = layerArray.Where(layer => layer != 0);

            foreach (int layer in layers)
            {
                if (Engine.Items.GetItem(layer, out Item i) && i.Layer != Layer.Invalid)
                {
                    itemList.Add(i);
                }
            }

            return itemList.ToArray();
        }
    }
}