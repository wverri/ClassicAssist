using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Resources;
using ClassicAssist.UO.Data;
using UOSteam;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Commands
    {
        public static void Register()
        {
            Interpreter.RegisterCommandHandler( "attack", AttackCommand );
            Interpreter.RegisterCommandHandler( "autoloot", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "bandageself", BandageSelfCommand );
            Interpreter.RegisterCommandHandler( "buy", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "cast", CastCommand );
            Interpreter.RegisterCommandHandler( "clearusequeue", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clearbuy", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clearhands", ClearHandsCommand );
            Interpreter.RegisterCommandHandler( "clearjournal", ClearJournalCommand );
            Interpreter.RegisterCommandHandler( "clearlist", ClearListCommand );
            Interpreter.RegisterCommandHandler( "clearsell", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clickobject", ClickObjectCommand );
            Interpreter.RegisterCommandHandler( "clickscreen", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "closegump", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "counter", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "createlist", CreateListCommand );
            Interpreter.RegisterCommandHandler( "dress", DressCommand );
            Interpreter.RegisterCommandHandler( "dressconfig", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "equipitem", EquipItemCommand );
            Interpreter.RegisterCommandHandler( "equipwand", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "feed", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "fly", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "guildbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "headmsg", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "helpbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "hotkeys", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "info", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "land", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "logoutbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "mapuo", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "messagebox", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "moveitem", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "moveitemoffset", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "movetype", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "movetypeoffset", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "msg", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "organizer", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "paperdoll", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "pause", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "ping", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "playmacro", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "playsound", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "poplist", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "promptalias", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "pushlist", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "questsbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "removelist", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "rename", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "replygump", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "resync", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "run", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "sell", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "setability", SetAbilityCommand );
            Interpreter.RegisterCommandHandler( "setalias", SetAliasCommand );
            Interpreter.RegisterCommandHandler( "shownames", ShowNamesCommand );
            Interpreter.RegisterCommandHandler( "snapshot", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "sysmsg", SysMsgCommand );
            Interpreter.RegisterCommandHandler( "toggleautoloot", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "togglehands", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "togglemounted", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "togglescavenger", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "turn", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "undress", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "unsetalias", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "useobject", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "useonce", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "useskill", UseSkillCommand );
            Interpreter.RegisterCommandHandler( "usetype", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "virtue", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "waitforgump", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "waitforjournal", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "walk", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "where", NotImplementedCommand );
        }

        private static bool SetAbilityCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if (args.Length == 0)
            {
                UOC.SystemMessage( "Usage: setability 'primary/secondary'" );
                return true;
            }

            AbilitiesCommands.SetAbility( args[0].AsString().ToLower() );

            return true;
        }

        private static bool UseSkillCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: useskill 'skill'" );
                return true;
            }

            SkillCommands.UseSkill( args[0].AsString().ToLower() );

            return true;
        }

        private static bool ShowNamesCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ActionCommands.ShowNames( args[0].AsString().ToLower() );
            return true;
        }

        private static bool SetAliasCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: setalias 'alias' value" );
                return true;
            }

            string alias = args[0].AsString();
            int serial = args[1].AsInt();

            AliasCommands.SetAlias( alias, serial );

            return true;
        }

        private static bool SysMsgCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: sysmsg 'text' [hue]" );
                return true;
            }

            string text = args[0].AsString();

            int hue = 946;

            if ( args.Length > 1 )
            {
                hue = args[1].AsInt();
            }

            UOC.SystemMessage( text, hue );

            return true;
        }

        private static bool EquipItemCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: equipitem serial layer" );
                return true;
            }

            uint serial = args[0].AsSerial();
            string layer = ( (Layer) args[1].AsInt() ).ToString();

            ActionCommands.EquipItem( serial, layer );

            return true;
        }

        private static bool DressCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                //TODO
            }
            else
            {
                string dressName = args[0].AsString();

                AgentCommands.Dress( dressName );
            }

            return true;
        }

        private static bool CreateListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: createlist 'list name'" );
                return true;
            }

            string listName = args[0].AsString();

            ListCommands.CreateList( listName );

            return true;
        }

        private static bool ClickObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: clickobject 'alias'/serial" );
                return true;
            }

            uint serial = args[0].AsSerial();

            ActionCommands.ClickObject( serial );

            return true;
        }

        private static bool ClearListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: clearlist 'list'" );
                return true;
            }

            string listName = args[0].AsString();

            ListCommands.ClearList( listName );

            return true;
        }

        private static bool ClearJournalCommand( string command, Argument[] args, bool quiet, bool force )
        {
            JournalCommands.ClearJournal();

            return true;
        }

        private static bool ClearHandsCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: clearhands 'left'/'right'/'both'" );
                return true;
            }

            string hands = args[0].AsString().ToLower();

            ActionCommands.ClearHands( hands );

            return true;
        }

        private static bool CastCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: cast 'spell'" );
                return true;
            }

            string spell = args[0].AsString().ToLower();

            SpellCommands.Cast( spell );

            return true;
        }

        private static bool BandageSelfCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ConsumeCommands.BandageSelf();

            return true;
        }

        private static bool AttackCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: attack 'alias'/serial" );
                return true;
            }

            uint serial = args[0].AsSerial();

            ActionCommands.Attack( serial );

            return true;
        }

        private static bool NotImplementedCommand( string command, Argument[] args, bool quiet, bool force )
        {
            UOC.SystemMessage( Strings.Command_currently_not_implemented_ );

            return true;
        }
    }
}