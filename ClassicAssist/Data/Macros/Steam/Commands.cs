using System;
using System.Linq;
using System.Threading;
using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Resources;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using UOScript;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Commands
    {
        public static void Register()
        {
            Interpreter.RegisterCommandHandler( "allymsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.AllyMsg ) );
            Interpreter.RegisterCommandHandler( "attack", AttackCommand );
            Interpreter.RegisterCommandHandler( "autoloot", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "bandageself", BandageSelfCommand );
            Interpreter.RegisterCommandHandler( "buy", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "cast", CastCommand );
            Interpreter.RegisterCommandHandler( "cancelprompt", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "chatmsg", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clearusequeue", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clearbuy", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clearhands", ClearHandsCommand );
            Interpreter.RegisterCommandHandler( "clearjournal", ClearJournalCommand );
            Interpreter.RegisterCommandHandler( "clearignorelist", ClearIgnoreListCommand );
            Interpreter.RegisterCommandHandler( "clearlist", ClearListCommand );
            Interpreter.RegisterCommandHandler( "clearsell", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "clickobject", ClickObjectCommand );
            Interpreter.RegisterCommandHandler( "clickscreen", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "closegump", CloseGumpCommand );
            Interpreter.RegisterCommandHandler( "contextmenu", ContextMenuCommand );
            Interpreter.RegisterCommandHandler( "createlist", CreateListCommand );
            Interpreter.RegisterCommandHandler( "dress", DressCommand );
            Interpreter.RegisterCommandHandler( "dressconfig", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "emotemsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.EmoteMsg ) );
            Interpreter.RegisterCommandHandler( "equipitem", EquipItemCommand );
            Interpreter.RegisterCommandHandler( "equipwand", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "feed", FeedCommand );
            Interpreter.RegisterCommandHandler( "fly", FlyCommand );
            Interpreter.RegisterCommandHandler( "guildbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "guildmsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.GuildMsg ) );
            Interpreter.RegisterCommandHandler( "headmsg", HeadMsgCommand );
            Interpreter.RegisterCommandHandler( "helpbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "hotkeys", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "info", InfoCommand );
            Interpreter.RegisterCommandHandler( "ignoreobject", IgnoreObjectCommand );
            Interpreter.RegisterCommandHandler( "land", LandCommand );
            Interpreter.RegisterCommandHandler( "logoutbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "mapuo", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "messagebox", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "moveitem", MoveItemCommand );
            Interpreter.RegisterCommandHandler( "moveitemoffset", MoveItemOffsetCommand );
            Interpreter.RegisterCommandHandler( "movetype", MoveTypeCommand );
            Interpreter.RegisterCommandHandler( "movetypeoffset", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "msg", MsgCommand );
            Interpreter.RegisterCommandHandler( "organizer", OrganizerCommand );
            Interpreter.RegisterCommandHandler( "paperdoll", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "partymsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.PartyMsg ) );
            Interpreter.RegisterCommandHandler( "pause", PauseCommand );
            Interpreter.RegisterCommandHandler( "ping", PingCommand );
            Interpreter.RegisterCommandHandler( "playmacro", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "playsound", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "poplist", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "promptalias", PromptAliasCommand );
            Interpreter.RegisterCommandHandler( "promptmsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.PromptMsg ) );
            Interpreter.RegisterCommandHandler( "pushlist", PushListCommand );
            Interpreter.RegisterCommandHandler( "questsbutton", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "removelist", RemoveListCommand );
            Interpreter.RegisterCommandHandler( "rename", RenameCommand );
            Interpreter.RegisterCommandHandler( "replygump", ReplyGumpCommand );
            Interpreter.RegisterCommandHandler( "resync", ResyncCommand );
            Interpreter.RegisterCommandHandler( "run", RunCommand );
            Interpreter.RegisterCommandHandler( "sell", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "setability", SetAbilityCommand );
            Interpreter.RegisterCommandHandler( "setalias", SetAliasCommand );
            Interpreter.RegisterCommandHandler( "shownames", ShowNamesCommand );
            Interpreter.RegisterCommandHandler( "snapshot", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "sysmsg", SysMsgCommand );
            Interpreter.RegisterCommandHandler( "toggleautoloot", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "togglehands", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "togglemounted", ToggleMountedCommand );
            Interpreter.RegisterCommandHandler( "togglescavenger", NotImplementedCommand );
            Interpreter.RegisterCommandHandler( "turn", TurnCommand );
            Interpreter.RegisterCommandHandler( "undress", UndressCommand );
            Interpreter.RegisterCommandHandler( "unsetalias", UnsetAliasCommand );
            Interpreter.RegisterCommandHandler( "useobject", UseObjectCommand );
            Interpreter.RegisterCommandHandler( "useonce", UseOnceCommand );
            Interpreter.RegisterCommandHandler( "useskill", UseSkillCommand );
            Interpreter.RegisterCommandHandler( "usetype", UseTypeCommand );
            Interpreter.RegisterCommandHandler( "virtue", VirtueCommand );
            Interpreter.RegisterCommandHandler( "waitforgump", WaitForGumpCommand );
            Interpreter.RegisterCommandHandler( "waitforjournal", WaitForJournalCommand );
            Interpreter.RegisterCommandHandler( "waitforproperties", WaitForPropertiesCommand );
            Interpreter.RegisterCommandHandler( "waitforprompt", WaitForPromptCommand );
            Interpreter.RegisterCommandHandler( "walk", WalkCommand );
            Interpreter.RegisterCommandHandler( "whispermsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.WhisperMsg ) );
            Interpreter.RegisterCommandHandler( "where", WhereCommand );
            Interpreter.RegisterCommandHandler( "yellmsg",
                ( c, a, q, f ) => MsgCommandNoHue( c, a, q, f, MsgCommands.YellMsg ) );
        }

        private static bool OrganizerCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "organizer ('profile name') [source] [destination]" );
            }

            string profile = args[0].AsString();
            object sourceContainer = args.Length > 1 ? (object) args[1].ResolveSerial() : null;
            object destinationContainer = args.Length > 2 ? (object) args[2].ResolveSerial() : null;

            OrganizerCommands.Organizer( profile, sourceContainer, destinationContainer );

            return true;
        }

        private static bool UseOnceCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "useonce (graphic) [color]" );
            }

            int id = args[0].AsInt();
            int hue = args.Length > 1 ? args[1].AsInt() : -1;

            ActionCommands.UseOnce( id, hue );

            return true;
        }

        private static bool TurnCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "run (direction)" );
            }

            string direction = args[0].AsString().ToLower();

            MovementCommands.Turn( direction );

            return true;
        }

        private static bool RunCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "run (direction)" );
            }

            string direction = args[0].AsString().ToLower();

            MovementCommands.Run( direction );

            return true;
        }

        private static bool WalkCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "walk (direction)" );
            }

            string direction = args[0].AsString().ToLower();

            MovementCommands.Walk( direction );

            return true;
        }

        private static bool AttackCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "attack 'alias'/serial" );
            }

            int serial = args[0].ResolveSerial();

            ActionCommands.Attack( serial );

            return true;
        }

        private static bool BandageSelfCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ConsumeCommands.BandageSelf();

            return true;
        }

        private static bool CastCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "cast 'spell'" );
            }

            string spell = args[0].AsString().ToLower();

            SpellCommands.Cast( spell );

            return true;
        }

        private static bool ClearHandsCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "clearhands 'left'/'right'/'both'" );
            }

            string hands = args[0].AsString().ToLower();

            ActionCommands.ClearHands( hands );

            return true;
        }

        private static bool ClearIgnoreListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ObjectCommands.ClearIgnoreList();

            return true;
        }

        private static bool ClearJournalCommand( string command, Argument[] args, bool quiet, bool force )
        {
            JournalCommands.ClearJournal();

            return true;
        }

        private static bool ClearListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "clearlist 'list'" );
            }

            string listName = args[0].AsString();

            ListCommands.ClearList( listName );

            return true;
        }

        private static bool ClickObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "clickobject 'alias'/serial" );
            }

            int serial = args[0].ResolveSerial();

            ActionCommands.ClickObject( serial );

            return true;
        }

        private static bool CloseGumpCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "closegump 'serial'" );
            }

            int serial = args[0].AsInt();

            GumpCommands.CloseGump( serial );
            return true;
        }

        private static bool ContextMenuCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "contextmenu (serial) (option)" );
            }

            int serial = args[0].ResolveSerial();
            int option = args[1].ResolveSerial();

            ActionCommands.ContextMenu( serial, option );

            return true;
        }

        private static bool CreateListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "createlist 'list name'" );
            }

            string listName = args[0].AsString();

            ListCommands.CreateList( listName );
            Interpreter.CreateList( listName );

            return true;
        }

        private static bool DisplayUsage( string usage )
        {
            UOC.SystemMessage( string.Format( Strings.Usage___0_, usage ) );

            // return true for now, throw SyntaxError? later
            return true;
        }

        private static bool DressCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "dress ['profile name']" );
            }

            string dressName = args[0].AsString();

            AgentCommands.Dress( dressName );

            return true;
        }

        private static bool EquipItemCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "equipitem serial layer" );
            }

            int serial = args[0].ResolveSerial();
            string layer = ( (Layer) args[1].AsInt() ).ToString();

            ActionCommands.EquipItem( serial, layer );

            return true;
        }

        private static bool FeedCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "feed (serial) ('graphic) [color] [amount]" );
            }

            int serial = args[0].ResolveSerial();
            int id = args[1].AsInt();
            int hue = args.Length > 2 ? args[2].AsInt() : -1;
            int amount = args.Length > 3 ? args[3].AsInt() : 1;

            ActionCommands.Feed( serial, id, amount, hue );

            return true;
        }

        private static bool FlyCommand( string command, Argument[] args, bool quiet, bool force )
        {
            AbilitiesCommands.Fly();

            return true;
        }

        private static bool HeadMsgCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 1 )
            {
                return DisplayUsage( "headmsg ('text') [color] [serial]" );
            }

            string text = args[0].AsString();
            int hue = args.Length > 1 ? args[1].AsInt() : 34;
            object serial = args.Length > 2 ? (object) args[2].ResolveSerial() : null;

            MsgCommands.HeadMsg( text, serial, hue );

            return true;
        }

        private static bool IgnoreObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "ignoreobject 'alias/serial'" );
            }

            int serial = args[0].ResolveSerial();

            ObjectCommands.IgnoreObject( serial );

            return true;
        }

        private static bool InfoCommand( string command, Argument[] args, bool quiet, bool force )
        {
            MainCommands.Info();

            return true;
        }

        private static bool LandCommand( string command, Argument[] args, bool quiet, bool force )
        {
            AbilitiesCommands.Land();
            return true;
        }

        private static bool MoveItemCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "moveitem (serial) (destination) [(x, y, z)] [amount]" );
            }

            int serial = args[0].ResolveSerial();
            int destination = args[1].ResolveSerial();
            int x = args.Length > 2 ? args[2].AsInt() : -1;
            int y = args.Length > 3 ? args[3].AsInt() : -1;
            int z = args.Length > 4 ? args[4].AsInt() : -1;
            int amount = args.Length > 5 ? args[5].AsInt() : -1;

            ActionCommands.MoveItem( serial, destination, amount, x, y );

            return true;
        }

        private static bool MoveItemOffsetCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "moveitemoffset (serial) 'ground' [(x, y, z)] [amount]" );
            }

            int serial = args[0].ResolveSerial();
            // Ignored
            string destination = args[1].AsString();
            int x = args.Length > 2 ? args[2].AsInt() : -1;
            int y = args.Length > 3 ? args[3].AsInt() : 0;
            int z = args.Length > 4 ? args[4].AsInt() : 0;
            int amount = args.Length > 5 ? args[5].AsInt() : -1;

            ObjectCommands.MoveItemOffset( serial, x, y, z, amount );

            return true;
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
            int x = args.Length > 3 ? args[3].AsInt() : -1;
            int y = args.Length > 4 ? args[4].AsInt() : -1;
            int z = args.Length > 5 ? args[5].AsInt() : -1;
            int hue = args.Length > 6 ? args[6].AsInt() : -1;
            int amount = args.Length > 7 ? args[7].AsInt() : -1;

            //TODO range

            ObjectCommands.MoveType( id, sourceSerial, destinationSerial, x, y, z, hue, amount );

            return true;
        }

        private static bool MsgCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 1 )
            {
                return DisplayUsage( "msg 'text' [hue]" );
            }

            string text = args[0].AsString();
            int hue = args.Length > 1 ? args[1].AsInt() : 34;

            MsgCommands.Msg( text, hue );

            return true;
        }

        private static bool MsgCommandNoHue( string command, Argument[] args, bool quiet, bool force,
            Action<string> msgCommand )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( $"{command} 'text'" );
            }

            string text = args[0].AsString();

            msgCommand.Invoke( text );

            return true;
        }

        private static bool NotImplementedCommand( string command, Argument[] args, bool quiet, bool force )
        {
            UOC.SystemMessage( string.Format( Strings.Command___0___currently_not_implemented___, command ) );

            return true;
        }

        private static bool PauseCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "pause 'timeout'" );
            }

            int timeout = args[0].AsInt();

            Thread.Sleep( timeout );
            return true;
        }

        private static bool PingCommand( string command, Argument[] args, bool quiet, bool force )
        {
            long[] results = new long[5];

            for ( int i = 0; i < 5; i++ )
            {
                results[i] = ActionCommands.Ping();

                UOC.SystemMessage( $"{Strings.Latency_} {results[i]} ms" );
            }

            long min = results.Min();
            long max = results.Max();
            double avg = results.Average();

            UOC.SystemMessage( string.Format( Strings.Min__0___Max__1___Average__2_, min, max, avg ) );

            return true;
        }

        private static bool PromptAliasCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "promptalias ('alias name')" );
            }

            string alias = args[0].AsString();

            AliasCommands.PromptAlias( alias );

            return true;
        }

        private static bool PushListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "pushlist ('list name') ('element value') ['front'/'back']" );
            }

            string listName = args[0].AsString();
            int val = args[1].AsInt();
            //TODO front/back

            ListCommands.PushList( listName, val );
            Interpreter.PushList( listName, args[1], false, true );
            return true;
        }

        private static bool RemoveListCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "removelist ('list name')" );
            }

            string listName = args[0].AsString();

            Interpreter.DestroyList( listName );
            ListCommands.RemoveList( listName );

            return true;
        }

        private static bool RenameCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "rename (serial) ('name')" );
            }

            int serial = args[0].ResolveSerial();
            string name = args[1].AsString();

            ActionCommands.Rename( serial, name );

            return true;
        }

        private static bool ReplyGumpCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "replygump (gump id/'any') (button) [option] [...]" );
            }

            uint id = args[0].AsUIntOrAny();
            int button = args[1].AsInt();
            //TODO options? are they switches?

            GumpCommands.ReplyGump( id, button );
            return true;
        }

        private static bool ResyncCommand( string command, Argument[] args, bool quiet, bool force )
        {
            MainCommands.Resync();

            return true;
        }

        private static bool SetAbilityCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "setability 'primary/secondary'" );
            }

            AbilitiesCommands.SetAbility( args[0].AsString().ToLower() );

            return true;
        }

        private static bool SetAliasCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "setalias 'alias' value" );
            }

            string alias = args[0].AsString();
            int serial = args[1].ResolveSerial();

            AliasCommands.SetAlias( alias, serial );

            return true;
        }

        private static bool ShowNamesCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ActionCommands.ShowNames( args[0].AsString().ToLower() );
            return true;
        }

        private static bool SysMsgCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "sysmsg 'text' [hue]" );
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

        private static bool ToggleMountedCommand( string command, Argument[] args, bool quiet, bool force )
        {
            ActionCommands.ToggleMounted();

            return true;
        }

        private static bool UndressCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "undress ['profile name']" );
            }

            string profile = args[0].AsString();

            AgentCommands.Undress( profile );

            return true;
        }

        private static bool UnsetAliasCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "unsetalias 'alias'" );
            }

            string alias = args[0].AsString();

            AliasCommands.UnsetAlias( alias );

            return true;
        }

        private static bool UseObjectCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "useobject 'serial/alias'" );
            }

            int serial = args[0].ResolveSerial();

            ObjectCommands.UseObject( serial );

            return true;
        }

        private static bool UseSkillCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "useskill 'skill'" );
            }

            SkillCommands.UseSkill( args[0].AsString().ToLower() );

            return true;
        }

        private static bool UseTypeCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 1 )
            {
                return DisplayUsage( "usetype (graphic) [color] [source] [range or search level]" );
            }

            int id = args[0].AsInt();
            int hue = args.Length > 1 ? args[1].AsInt() : -1;
            object source = args.Length > 2 ? (object) args[2].ResolveSerial() : null;

            ObjectCommands.UseType( id, hue, source );

            return true;
        }

        private static bool VirtueCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "virtue ('honor'/'sacrifice'/'valor')" );
            }

            string virtue = args[0].AsString();

            MainCommands.InvokeVirtue( virtue );

            return true;
        }

        private static bool WaitForGumpCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "waitforgump 'id/any' (timeout)" );
            }

            uint id = args[0].AsUIntOrAny();
            int timeout = args[1].AsInt();

            GumpCommands.WaitForGump( id, timeout );

            return true;
        }

        private static bool WaitForJournalCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "waitforjournal ('text') (timeout) ['author'/'system']" );
            }

            string text = args[0].AsString();
            int timeout = args[1].AsInt();
            string author = args.Length > 2 ? args[2].AsString() : string.Empty;

            JournalCommands.WaitForJournal( text, timeout, author );

            return true;
        }

        private static bool WaitForPromptCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length == 0 )
            {
                return DisplayUsage( "waitforprompt (timeout) " );
            }

            int timeout = args[0].AsInt();

            MsgCommands.WaitForPrompt( timeout );

            return true;
        }

        private static bool WaitForPropertiesCommand( string command, Argument[] args, bool quiet, bool force )
        {
            if ( args.Length < 2 )
            {
                return DisplayUsage( "waitforproperties (serial) (timeout)" );
            }

            int serial = args[0].ResolveSerial();
            int timeout = args[1].AsInt();

            PropertiesCommands.WaitForProperties( serial, timeout );
            return true;
        }

        private static bool WhereCommand( string command, Argument[] args, bool quiet, bool force )
        {
            PlayerMobile player = Engine.Player;

            if ( player == null )
            {
                return true;
            }

            UOC.SystemMessage( $"{Strings.Current_Location_} {player.X}, {player.Y}, {player.Map}" );
            UOC.SystemMessage( $"Region: {Regions.Regions.GetRegion( Engine.Player )}" );

            return true;
        }
    }
}