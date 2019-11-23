using ClassicAssist.UO.Data;

namespace ClassicAssist.UO.Objects
{
    public class PlayerMobile : Mobile
    {
        public PlayerMobile( int serial ) : base( serial )
        {
        }

        public int Backpack => GetLayer( Layer.Backpack );
        public Map Map { get; set; }
    }
}