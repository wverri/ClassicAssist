using ClassicAssist.UO.Data;

namespace ClassicAssist.UO.Objects
{
    public class Item : Entity
    {
        private string _name;

        public Item( int serial ) : base( serial )
        {
        }

        public Item(int serial, int containerSerial) : base(serial)
        {
            Owner = containerSerial;
        }

        public int Owner { get; set; }
        public ItemCollection Container { get; set; }
        public bool IsContainer => Container != null;
        public int ArtDataID { get; set; }
        public int Count { get; set; }
        public int Flags { get; set; }
        public int Light { get; set; }
        public int Grid { get; set; }
        public int Layer { get; set; }

        public override string Name
        {
            get => string.IsNullOrEmpty(_name) ? TileData.GetStaticTile( ID ).Name : _name;
            set => _name = value;
        }
    }
}