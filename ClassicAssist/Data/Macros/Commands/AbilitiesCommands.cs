using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assistant;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using Newtonsoft.Json;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class AbilitiesCommands
    {
        private static List<WeaponData> _weaponData;

        [CommandsDisplay(Category = "Abilities", Description = "Clear weapon ability.")]
        public static void ClearAbility()
        {
            UOC.ClearWeaponAbility();
        }

        [CommandsDisplay( Category = "Abilities",
            Description = "Set weapon ability, parameter \"primary\" / \"secondary\"." )]
        public static void SetAbility( string ability )
        {
            // TODO stun/disarm old
            bool primary;

            switch ( ability.ToLower() )
            {
                case "primary":
                    primary = true;
                    break;
                default:
                    primary = false;
                    break;
            }

            if ( _weaponData == null )
            {
                LoadWeaponData( Engine.StartupPath );
            }

            if ( Engine.Player == null )
            {
                return;
            }

            int twoHandSerial = Engine.Player.GetLayer( Layer.TwoHanded );

            Item twoHandItem = Engine.Items.GetItem( twoHandSerial );

            if ( twoHandItem != null )
            {
                WeaponData wd =
                    ( _weaponData ?? throw new InvalidOperationException() ).FirstOrDefault( d =>
                        d.Graphic == twoHandItem.ID && d.Twohanded );

                if ( wd != null )
                {
                    UOC.SetWeaponAbility( primary ? wd.Primary : wd.Secondary );
                    return;
                }
            }

            int oneHandSerial = Engine.Player.GetLayer( Layer.OneHanded );

            Item oneHandItem = Engine.Items.GetItem( oneHandSerial );

            if ( oneHandItem != null )
            {
                WeaponData wd =
                    ( _weaponData ?? throw new InvalidOperationException() ).FirstOrDefault( d =>
                        d.Graphic == oneHandItem.ID && !d.Twohanded );

                if ( wd != null )
                {
                    UOC.SetWeaponAbility( primary ? wd.Primary : wd.Secondary );
                    return;
                }
            }

            // Fists etc
            UOC.SetWeaponAbility( primary ? 11 : 5 );

        }

        [CommandsDisplay( Category = "Abilities", Description = "(Garoyle) Start flying if not already flying." )]
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

        [CommandsDisplay( Category = "Abilities", Description = "(Garoyle) Stop flying if currently flying." )]
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

        private static void LoadWeaponData( string basePath )
        {
            string dataPath = Path.Combine( basePath, "Data" );

            _weaponData = JsonConvert
                .DeserializeObject<WeaponData[]>( File.ReadAllText( Path.Combine( dataPath, "Weapons.json" ) ) )
                .ToList();
        }

        internal class WeaponData
        {
            public int Graphic { get; set; }
            public string Name { get; set; }
            public int Primary { get; set; }
            public int Secondary { get; set; }
            public bool Twohanded { get; set; }
        }
    }
}