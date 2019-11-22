namespace ClassicAssist.UO.Objects
{
    public class Mobile : Entity
    {
        public Mobile( int serial ) : base( serial )
        {
        }

        public int Hits { get; set; }
        public int HitsMax { get; set; }
        public int Notoriety { get; set; }
        public int Status { get; set; }
    }
}