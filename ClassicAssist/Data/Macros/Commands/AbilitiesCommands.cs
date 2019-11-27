using System;
using Assistant;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class AbilitiesCommands
    {
        public static void SetWeaponAbility( object obj )
        {
            //TODO
            throw new NotImplementedException();
        }

        public static void Fly()
        {
            PlayerMobile player = Engine.Player;

            if ( player == null )
            {
                return;
            }

            if ( !player.Status.HasFlag( MobileStatus.Flying ) )
            {
                UOC.ToggleGargoyleFlying();
            }
        }

        public static void Land()
        {
            PlayerMobile player = Engine.Player;

            if ( player == null )
            {
                return;
            }

            if ( player.Status.HasFlag( MobileStatus.Flying ) )
            {
                UOC.ToggleGargoyleFlying();
            }
        }
    }
}