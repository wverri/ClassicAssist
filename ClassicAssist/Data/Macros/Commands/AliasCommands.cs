using System.Collections.Generic;
using Assistant;
using ClassicAssist.Resources;
using ClassicAssist.UO;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class AliasCommands
    {
        public static Dictionary<string, int> _aliases = new Dictionary<string, int>();

        internal static int ResolveSerial( object obj )
        {
            int serial = 0;

            switch ( obj )
            {
                case string str:
                    serial = GetAlias( str );

                    if ( serial == -1 )
                    {
                        UOC.SystemMessage( string.Format( Strings.Unknown_alias___0___, str ) );
                    }

                    break;
                case int i:
                    serial = i;
                    break;
                default:
                    UOC.SystemMessage( Strings.Invalid_or_unknown_object_id );
                    return -1;
            }

            return serial;
        }

        public static void SetAlias( string aliasName, int value )
        {
            if ( _aliases.ContainsKey( aliasName ) )
            {
                _aliases[aliasName] = value;
            }
            else
            {
                _aliases.Add( aliasName, value );
            }
        }

        public static void UnsetAlias( string aliasName )
        {
            if ( _aliases.ContainsKey( aliasName ) )
            {
                _aliases.Remove( aliasName );
            }
        }

        public static int GetAlias( string aliasName )
        {
            if ( _aliases.ContainsKey( aliasName ) )
            {
                return _aliases[aliasName];
            }

            return -1;
        }

        public static void PromptAlias( string aliasName )
        {
            int serial = UOC.GetTargeSerialAsync( Strings.Target_object___, 30000 ).Result;
        }

        public static bool FindAlias( string aliasName )
        {
            int serial;

            if ( ( serial = GetAlias( aliasName ) ) == -1 )
            {
                return false;
            }

            if ( UOMath.IsMobile( serial ) )
            {
                return Engine.Mobiles.GetMobile( serial ) != null;
            }

            return Engine.Items.GetItem( serial ) != null;
        }
    }
}