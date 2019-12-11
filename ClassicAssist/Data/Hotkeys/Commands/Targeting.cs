using System.Collections.Generic;
using System.Linq;
using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.Data.Hotkeys.Commands
{
    public class Targeting
    {
        private const double MAX_DISTANCE = 12;
        private static readonly List<Mobile> _ignoreList = new List<Mobile>();

        private static void SetTarget( Entity m )
        {
            MsgCommands.HeadMsg( "[Target]", m.Serial );
            MsgCommands.HeadMsg( $"Target: {m.Name?.Trim() ?? "Unknown"}" );
            Engine.Player.LastTargetSerial = m.Serial;
        }

        public static Mobile GetNextMobile( IEnumerable<Notoriety> notoriety, bool recurring = false )
        {
            while ( true )
            {
                Mobile[] mobiles = Engine.Mobiles.SelectEntities( m =>
                    notoriety.Contains( m.Notoriety ) && m.Distance < MAX_DISTANCE && !_ignoreList.Contains( m ) );

                if ( mobiles == null || mobiles.Length == 0 )
                {
                    _ignoreList.Clear();

                    if ( recurring )
                    {
                        return null;
                    }

                    recurring = true;
                    continue;
                }

                Mobile mobile = mobiles.FirstOrDefault();
                _ignoreList.Add( mobile );
                return mobile;
            }
        }

        [HotkeyCommand( Name = "Get Next Gray", Category = "Targeting" )]
        public class GetNextGray : HotkeyCommand
        {
            public override void Execute()
            {
                Mobile m = GetNextMobile( new[] { Notoriety.Criminal, Notoriety.Attackable } );

                if ( m == null )
                {
                    return;
                }

                SetTarget( m );
            }
        }

        [HotkeyCommand( Name = "Get Next Innocent", Category = "Targeting" )]
        public class GetNextInnocent : HotkeyCommand
        {
            public override void Execute()
            {
                Mobile m = GetNextMobile( new[] { Notoriety.Innocent } );

                if ( m == null )
                {
                    return;
                }

                SetTarget( m );
            }
        }
    }
}