using System.Collections.Generic;
using System.Linq;
using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Resources;
using ClassicAssist.UO;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network.Packets;
using ClassicAssist.UO.Objects;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Hotkeys.Commands
{
    public class Targeting
    {
        private const int MAX_DISTANCE = 18;
        private static readonly List<Mobile> _ignoreList = new List<Mobile>();

        private static void SetEnemy( Entity m )
        {
            if ( !UOMath.IsMobile( m.Serial ) )
            {
                return;
            }

            MsgCommands.HeadMsg( "[Target]", m.Serial );
            MsgCommands.HeadMsg( $"Target: {m.Name?.Trim() ?? "Unknown"}" );
            Engine.Player.LastTargetSerial = m.Serial;
            Engine.Player.EnemyTargetSerial = m.Serial;
            Engine.SendPacketToClient( new ChangeCombatant( m.Serial ) );
        }

        private static void SetLastTarget( Entity m )
        {
            Engine.Player.LastTargetSerial = m.Serial;

            if ( !UOMath.IsMobile( m.Serial ) )
            {
                return;
            }

            MsgCommands.HeadMsg( "[Target]", m.Serial );
            MsgCommands.HeadMsg( $"Target: {m.Name?.Trim() ?? "Unknown"}" );
            Engine.SendPacketToClient( new ChangeCombatant( m.Serial ) );
        }

        public static Mobile GetClosestMobile( IEnumerable<Notoriety> notoriety )
        {
            Mobile mobile = Engine.Mobiles.SelectEntities( m =>
                    notoriety.Contains( m.Notoriety ) && m.Distance < MAX_DISTANCE ).OrderBy( m => m.Distance )
                .FirstOrDefault();

            return mobile;
        }

        public static Mobile GetNextEnemy( IEnumerable<Notoriety> notoriety, bool noFriends = false,
            int distance = MAX_DISTANCE )
        {
            bool recurring = false;

            while ( true )
            {
                Mobile[] mobiles = Engine.Mobiles.SelectEntities( m =>
                    notoriety.Contains( m.Notoriety ) && m.Distance < distance && !_ignoreList.Contains( m ) &&
                    ( !noFriends || !MobileCommands.InFriendList( m.Serial ) ) );

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

        public static Mobile GetNextGray()
        {
            Mobile m = GetNextEnemy( new[] { Notoriety.Criminal, Notoriety.Attackable } );

            if ( m == null )
            {
                return null;
            }

            SetEnemy( m );

            return m;
        }

        public static Mobile GetNextInnocent()
        {
            Mobile m = GetNextEnemy( new[] { Notoriety.Innocent } );

            if ( m == null )
            {
                return null;
            }

            SetEnemy( m );

            return m;
        }

        public static Mobile GetNextMurderer()
        {
            Mobile m = GetNextEnemy( new[] { Notoriety.Murderer } );

            if ( m == null )
            {
                return null;
            }

            SetEnemy( m );

            return m;
        }

        public static Mobile GetNextUnfriendly()
        {
            Mobile m = GetNextEnemy(
                new[] { Notoriety.Murderer, Notoriety.Attackable, Notoriety.Criminal, Notoriety.Enemy }, true );

            if ( m == null )
            {
                return null;
            }

            SetEnemy( m );

            return m;
        }

        private static Entity PromptTarget()
        {
            int serial = UOC.GetTargeSerialAsync( Strings.Target_object___ ).Result;

            if ( serial == 0 )
            {
                UOC.SystemMessage( Strings.Invalid_or_unknown_object_id );
                return null;
            }

            Mobile mobile = Engine.Mobiles.GetMobile( serial );

            if ( mobile != null )
            {
                return mobile;
            }

            UOC.SystemMessage( Strings.Mobile_not_found___ );
            return null;
        }

        [HotkeyCommand( Name = "Set Enemy", Category = "Targeting" )]
        public class SetEnemyCommand : HotkeyCommand
        {
            public override void Execute()
            {
                Entity mobile = PromptTarget();

                if ( mobile == null )
                {
                    return;
                }

                SetEnemy( mobile );
                Engine.SendPacketToServer( new LookRequest( mobile.Serial ) );
            }
        }

        [HotkeyCommand( Name = "Set Last Target", Category = "Targeting" )]
        public class SetLastTargetCommand : HotkeyCommand
        {
            public override void Execute()
            {
                Entity entity = PromptTarget();

                if ( entity == null )
                {
                    return;
                }

                SetLastTarget( entity );

                if ( UOMath.IsMobile( entity.Serial ) )
                {
                    Engine.SendPacketToServer( new LookRequest( entity.Serial ) );
                }
            }
        }

        [HotkeyCommand( Name = "Get Next Gray", Category = "Targeting" )]
        public class GetNextGrayCommand : HotkeyCommand
        {
            public override void Execute()
            {
                GetNextGray();
            }
        }

        [HotkeyCommand( Name = "Get Next Innocent", Category = "Targeting" )]
        public class GetNextInnocentCommand : HotkeyCommand
        {
            public override void Execute()
            {
                GetNextInnocent();
            }
        }

        [HotkeyCommand( Name = "Get Next Murderer", Category = "Targeting" )]
        public class GetNextMurdererCommand : HotkeyCommand
        {
            public override void Execute()
            {
                GetNextMurderer();
            }
        }

        [HotkeyCommand( Name = "Get Next Unfriendly", Category = "Targeting" )]
        public class GetNextUnfriendlyCommand : HotkeyCommand
        {
            public override void Execute()
            {
                GetNextUnfriendly();
            }
        }
    }
}