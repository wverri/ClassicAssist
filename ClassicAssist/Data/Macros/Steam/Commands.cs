using System.Threading;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Resources;
using ClassicAssist.UO.Data;
using UOScript;
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
            Interpreter.RegisterCommandHandler( "clearignorelist", ClearIgnoreListCommand );
            Interpreter.RegisterCommandHandler( "clearlist", ClearListCommand );
            Interpreter.RegisterCommandHandler( "clearsell", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clickobject", ClickObjectCommand );
            Interpreter.RegisterCommandHandler( "clickscreen", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "closegump", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "contextmenu", ContextMenuCommand );
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
            Interpreter.RegisterCommandHandler( "ignoreobject", IgnoreObjectCommand );
            Interpreter.RegisterCommandHandler( "land", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "logoutbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "mapuo", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "messagebox", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "moveitem", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "moveitemoffset", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "movetype", MoveTypeCommand );
            Interpreter.RegisterCommandHandler( "movetypeoffset", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "msg", MsgCommand );
            Interpreter.RegisterCommandHandler( "organizer", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "paperdoll", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "pause", PauseCommand );
            Interpreter.RegisterCommandHandler( "ping", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "playmacro", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "playsound", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "poplist", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "promptalias", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "pushlist", PushListCommand );
            Interpreter.RegisterCommandHandler( "questsbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "removelist", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "rename", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "replygump", ReplyGumpCommand );
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
            Interpreter.RegisterCommandHandler( "unsetalias", UnsetAliasCommand );
            Interpreter.RegisterCommandHandler( "useobject", UseObjectCommand );
            Interpreter.RegisterCommandHandler( "useonce", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "useskill", UseSkillCommand );
            Interpreter.RegisterCommandHandler( "usetype", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "virtue", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "waitforgump", WaitForGumpCommand );
            Interpreter.RegisterCommandHandler( "waitforjournal", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "waitforproperties", WaitForPropertiesCommand );
            Interpreter.RegisterCommandHandler( "walk", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "where", NotImplementedCommand );
        }

        private static bool MoveTypeCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 3 )
            {
                UOC.SystemMessage(
                    "Usage: movetype (graphic) (source) (destination) [(x, y, z)] [color] [amount] [range or search level]" );
                return true;
            }

            int id = args[0].AsInt();
            int sourceSerial = args[1].ResolveSerial();
            int destinationSerial = args[2].ResolveSerial();

            ObjectCommands.MoveType( id, sourceSerial, destinationSerial );

            return true;
        }

        private static bool ContextMenuCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: contextmenu (serial) (option)" );
                return true;
            }

            int serial = args[0].ResolveSerial();
            int option = args[1].ResolveSerial();

            ActionCommands.ContextMenu( serial, option );

            return true;
        }

        private static bool MsgCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 1 )
            {
                UOC.SystemMessage( "Usage: msg 'text' [hue]" );
                return true;
            }

            string text = args[0].AsString();
            int hue = args.Length > 1 ? args[1].AsInt() : 34;

            MsgCommands.Msg( text, hue );

            return true;
        }

        private static bool PauseCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: pause 'timeout'" );
                return true;
            }

            int timeout = args[0].AsInt();

            Thread.Sleep( timeout );
            return true;
        }

        private static bool ReplyGumpCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: replygump (gump id/'any') (button) [option] [...]" );
                return true;
            }

            uint id = args[0].AsUIntOrAny();
            int button = args[1].AsInt();
            //TODO options? are they switches?

            GumpCommands.ReplyGump( id, button );
            return true;
        }

        private static bool WaitForGumpCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: waitforgump 'id/any' (timeout)" );
                return true;
            }

            uint id = args[0].AsUIntOrAny();
            int timeout = args[1].AsInt();

            GumpCommands.WaitForGump( id, timeout );

            return true;
        }

        private static bool WaitForPropertiesCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: waitforproperties (serial) (timeout)" );
                return true;
            }

            int serial = args[0].ResolveSerial();
            int timeout = args[1].AsInt();

            PropertiesCommands.WaitForProperties( serial, timeout );
            return true;
        }

        private static bool IgnoreObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: ignoreobject 'alias/serial'" );
                return true;
            }

            int serial = args[0].ResolveSerial();

            ObjectCommands.IgnoreObject( serial );

            return true;
        }

        private static bool PushListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: pushlist ('list name') ('element value') ['front'/'back']" );
                return true;
            }

            string listName = args[0].AsString();
            int val = args[1].AsInt();
            //TODO front/back

            ListCommands.PushList( listName, val );
            Interpreter.PushList( listName, args[1], false, true );
            return true;
        }

        private static bool ClearIgnoreListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ObjectCommands.ClearIgnoreList();

            return true;
        }

        private static bool UseObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: useobject 'serial/alias'" );
                return true;
            }

            int serial = args[0].ResolveSerial();

            ObjectCommands.UseObject( serial );

            return true;
        }

        private static bool UnsetAliasCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: unsetalias 'alias'" );
                return true;
            }

            string alias = args[0].AsString();

            AliasCommands.UnsetAlias( alias );

            return true;
        }

        private static bool SetAbilityCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
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
            int serial = args[1].ResolveSerial();

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

            int serial = args[0].ResolveSerial();
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
            Interpreter.CreateList( listName );

            return true;
        }

        private static bool ClickObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: clickobject 'alias'/serial" );
                return true;
            }

            int serial = args[0].ResolveSerial();

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

            int serial = args[0].ResolveSerial();

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