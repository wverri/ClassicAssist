#region License

// Copyright (C) 2020 Reetus
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Data.Regions;
using ClassicAssist.Resources;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using UOScript;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Expressions
    {
        public static void Register()
        {
            Interpreter.RegisterExpressionHandler( "buffexists", BuffExistsExpression );
            Interpreter.RegisterExpressionHandler( "contents", ContentsExpression );
            Interpreter.RegisterExpressionHandler( "counter", CounterExpression );
            Interpreter.RegisterExpressionHandler( "counttype", CountTypeExpression );
            Interpreter.RegisterExpressionHandler( "counttypeground", CountTypeGroundExpression );
            Interpreter.RegisterExpressionHandler( "distance", DistanceExpression );
            Interpreter.RegisterExpressionHandler( "dressing", DressingExpression );
            Interpreter.RegisterExpressionHandler( "findalias", FindAliasExpression );
            Interpreter.RegisterExpressionHandler( "findlayer", FindLayerExpression );
            Interpreter.RegisterExpressionHandler( "findobject", FindObjectExpression );
            Interpreter.RegisterExpressionHandler( "findtype", FindTypeExpression );
            Interpreter.RegisterExpressionHandler( "findwand", NotImplementedExpression );
            Interpreter.RegisterExpressionHandler( "flying", FlyingExpression );
            Interpreter.RegisterExpressionHandler( "gumpexists", GumpExistsExpression );
            Interpreter.RegisterExpressionHandler( "infriendslist", InFriendsListExpression );
            Interpreter.RegisterExpressionHandler( "ingump", InGumpExpression );
            Interpreter.RegisterExpressionHandler( "injournal", InJournalExpression );
            Interpreter.RegisterExpressionHandler( "inlist", InListExpression );
            Interpreter.RegisterExpressionHandler( "inparty", InPartyExpression );
            Interpreter.RegisterExpressionHandler( "inrange", InRangeExpression );
            Interpreter.RegisterExpressionHandler( "inregion", InRegionExpression );
            Interpreter.RegisterExpressionHandler( "list", ListLengthExpression );
            Interpreter.RegisterExpressionHandler( "listexists", ListExistsExpression );
            Interpreter.RegisterExpressionHandler( "organizing", OrganizingExpression );
            Interpreter.RegisterExpressionHandler( "property", PropertyExpression );
            Interpreter.RegisterExpressionHandler( "skill", SkillExpression );
            Interpreter.RegisterExpressionHandler( "skillstate", SkillStateExpression );
            Interpreter.RegisterExpressionHandler( "timer", TimerExpression );
            Interpreter.RegisterExpressionHandler( "timerexists", TimerExistsExpression );
            Interpreter.RegisterExpressionHandler( "war", WarExpression );

            // Player Attributes
            Interpreter.RegisterExpressionHandler( "str", ( e, a, q ) => MobileCommands.Str() );
            Interpreter.RegisterExpressionHandler( "int", ( e, a, q ) => MobileCommands.Int() );
            Interpreter.RegisterExpressionHandler( "dex", ( e, a, q ) => MobileCommands.Dex() );
            Interpreter.RegisterExpressionHandler( "mana", ( e, a, q ) => MobileCommands.Mana() );
            Interpreter.RegisterExpressionHandler( "maxmana", ( e, a, q ) => MobileCommands.MaxMana() );
            Interpreter.RegisterExpressionHandler( "stam", ( e, a, q ) => MobileCommands.Stam() );
            Interpreter.RegisterExpressionHandler( "maxstam", ( e, a, q ) => MobileCommands.MaxStam() );
            Interpreter.RegisterExpressionHandler( "weight", ( e, a, q ) => MobileCommands.Weight() );
            Interpreter.RegisterExpressionHandler( "maxweight", ( e, a, q ) => MobileCommands.MaxWeight() );
            Interpreter.RegisterExpressionHandler( "diffweight", ( e, a, q ) => MobileCommands.DiffWeight() );
            Interpreter.RegisterExpressionHandler( "luck", ( e, a, q ) => MobileCommands.Luck() );
            Interpreter.RegisterExpressionHandler( "tithingpoints", ( e, a, q ) => MobileCommands.TithingPoints() );
            Interpreter.RegisterExpressionHandler( "followers", ( e, a, q ) => MobileCommands.Followers() );
            Interpreter.RegisterExpressionHandler( "maxfollowers", ( e, a, q ) => MobileCommands.MaxFollowers() );

            // Mobile attributes
            Interpreter.RegisterExpressionHandler( "x",
                ( e, a, q ) => MobileAtrribute<int>( e, a, q, nameof( Mobile.X ) ) );
            Interpreter.RegisterExpressionHandler( "y",
                ( e, a, q ) => MobileAtrribute<bool>( e, a, q, nameof( Mobile.Y ) ) );
            Interpreter.RegisterExpressionHandler( "z",
                ( e, a, q ) => MobileAtrribute<bool>( e, a, q, nameof( Mobile.Z ) ) );
            Interpreter.RegisterExpressionHandler( "serial",
                ( e, a, q ) => MobileAtrribute<int>( e, a, q, nameof( Mobile.Serial ) ) );
            Interpreter.RegisterExpressionHandler( "dead",
                ( e, a, q ) => MobileAtrribute<bool>( e, a, q, nameof( Mobile.IsDead ) ) );
            Interpreter.RegisterExpressionHandler( "paralyzed",
                ( e, a, q ) => MobileAtrribute<bool>( e, a, q, nameof( Mobile.IsFrozen ) ) );
            Interpreter.RegisterExpressionHandler( "poisoned",
                ( e, a, q ) => MobileAtrribute<bool>( e, a, q, nameof( Mobile.IsPoisoned ) ) );
            Interpreter.RegisterExpressionHandler( "yellowhits",
                ( e, a, q ) => MobileAtrribute<bool>( e, a, q, nameof( Mobile.IsYellowHits ) ) );
            Interpreter.RegisterExpressionHandler( "hits",
                ( e, a, q ) => MobileCommandResult( e, a, q, MobileCommands.Hits ) );
            Interpreter.RegisterExpressionHandler( "maxhits",
                ( e, a, q ) => MobileCommandResult( e, a, q, MobileCommands.MaxHits ) );
            Interpreter.RegisterExpressionHandler( "diffhits",
                ( e, a, q ) => MobileCommandResult( e, a, q, MobileCommands.DiffHits ) );
            Interpreter.RegisterExpressionHandler( "criminal",
                ( e, a, q ) => MobileCommandResult( e, a, q, NotorietyCommands.Criminal ) );
            Interpreter.RegisterExpressionHandler( "enemy",
                ( e, a, q ) => MobileCommandResult( e, a, q, NotorietyCommands.Enemy ) );
            Interpreter.RegisterExpressionHandler( "friend",
                ( e, a, q ) => MobileCommandResult( e, a, q, MobileCommands.InFriendList ) );
            Interpreter.RegisterExpressionHandler( "gray",
                ( e, a, q ) => MobileCommandResult( e, a, q, NotorietyCommands.Gray ) );
            Interpreter.RegisterExpressionHandler( "innocent",
                ( e, a, q ) => MobileCommandResult( e, a, q, NotorietyCommands.Innocent ) );
            Interpreter.RegisterExpressionHandler( "invulnerable",
                ( e, a, q ) => MobileCommandResult( e, a, q, NotorietyCommands.Invulnerable ) );
            Interpreter.RegisterExpressionHandler( "murderer",
                ( e, a, q ) => MobileCommandResult( e, a, q, NotorietyCommands.Murderer ) );

            //TODO
            Interpreter.RegisterExpressionHandler( "name", UnsupportedComparisonToString );
            Interpreter.RegisterExpressionHandler( "direction", UnsupportedComparisonToString );
        }

        private static double MobileCommandResult<T>( string expression, Argument[] args, bool quiet,
            Func<object, T> command )
        {
            // Optional parameter or will get PlayerMobile attribute
            object serial = args.Length > 0 ? (object) args[0].ResolveSerial() : null;

            T val = command.Invoke( serial );

            if ( val is bool valBool )
            {
                return valBool ? 1 : 0;
            }

            int result = (int) Convert.ChangeType( val, typeof( int ) );

            return result;
        }

        private static double MobileAtrribute<T>( string expression, IReadOnlyList<Argument> args, bool quiet,
            string attribute )
        {
            // Optional parameter or will get PlayerMobile attribute
            object serial = args.Count > 0 ? (object) args[0].ResolveSerial() : null;

            T val = MobileCommands.GetMobileProperty<T>( serial, attribute );

            if ( val is bool valBool )
            {
                return valBool ? 1 : 0;
            }

            int result = (int) Convert.ChangeType( val, typeof( int ) );

            return result;
        }

        private static double DressingExpression( string expression, Argument[] args, bool quiet )
        {
            return AgentCommands.Dressing() ? 1 : 0;
        }

        private static double OrganizingExpression( string expression, Argument[] args, bool quiet )
        {
            return OrganizerCommands.Organizing() ? 1 : 0;
        }

        private static double FlyingExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: flying 'serial/alias'" );
                return 0;
            }

            int serial = args[0].ResolveSerial();

            return AbilitiesCommands.Flying( serial ) ? 1 : 0;
        }

        private static double CounterExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: counter ('format') (operator) (value)" );
                return 0;
            }

            string counter = args[0].AsString();

            return AgentCommands.Counter( counter );
        }

        private static double WarExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: if war (serial)" );
                return 0;
            }

            int serial = args[0].ResolveSerial();

            return MobileCommands.War( serial ) ? 1 : 0;
        }

        private static double TimerExistsExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: if timerexists ('timer name')" );
                return 0;
            }

            string timerName = args[0].AsString();

            return TimerCommands.TimerExists( timerName ) ? 1 : 0;
        }

        private static double TimerExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: if timer ('timer name') (operator) (value)" );
                return 0;
            }

            string timerName = args[0].AsString();

            return TimerCommands.Timer( timerName );
        }

        private static double SkillStateExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: skillstate ('skill name') (operator) ('locked'/'up'/'down')" );
                return 0;
            }

            //TODO ??
            return 0;
        }

        private static double PropertyExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: property ('name') (serial)" );
            }

            string property = args[0].AsString();
            int serial = args[1].ResolveSerial();

            if ( serial == 0 )
            {
                return 0;
            }

            Entity entity = (Entity) Engine.Items.GetItem( serial ) ?? Engine.Mobiles.GetMobile( serial );

            if ( entity.Properties != null )
            {
                Property p = entity.Properties.FirstOrDefault( pe => pe.Text.ToLower().Contains( property.ToLower() ) );

                // Property doesn't exist
                if ( p == null )
                {
                    return 0;
                }

                //Property exists but no argument or argument is string
                if ( !int.TryParse( p.Arguments[0], out _ ) )
                {
                    return 1;
                }

                return int.Parse( p.Arguments[0] );
            }

            return 0;
        }

        private static double InRegionExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage:  inregion ('guards'/'town'/'dungeon'/'forest') [serial] [range]" );
            }

            string regionName = args[0].AsString();

            RegionAttributes attr = RegionAttributes.None;

            switch ( regionName.ToLower() )
            {
                case "guards":
                    attr = RegionAttributes.Guarded;
                    break;
                case "town":
                    attr = RegionAttributes.Town;
                    break;
                case "dungeon":
                    attr = RegionAttributes.Dungeon;
                    break;
                case "forest":
                    attr = RegionAttributes.Wilderness;
                    break;
            }

            int serial = args[1].ResolveSerial();

            if ( attr == RegionAttributes.None )
            {
                return 0;
            }

            //TODO range

            return RegionCommands.InRegion( attr.ToString(), serial ) ? 1 : 0;
        }

        private static double InRangeExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage:  inrange (serial) (range)" );
                return 0;
            }

            int serial = args[0].ResolveSerial();
            int range = args[1].AsInt();

            return EntityCommands.InRange( serial, range ) ? 1 : 0;
        }

        private static double InPartyExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: inparty (serial)" );
                return 0;
            }

            int serial = args[0].ResolveSerial();

            return MobileCommands.InParty( serial ) ? 1 : 0;
        }

        private static double InGumpExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: ingump (gump id/'any') ('text')" );
                return 0;
            }

            uint id = args[0].AsUIntOrAny();
            string text = args[1].AsString();

            return GumpCommands.InGump( id, text ) ? 1 : 0;
        }

        private static double InFriendsListExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: infriendlist (serial/alias)" );
                return 0;
            }

            int serial = args[0].ResolveSerial();

            return MobileCommands.InFriendList( serial ) ? 1 : 0;
        }

        private static double GumpExistsExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage:  gumpexists (gump id/'any')" );
                return 0;
            }

            uint id = args[0].AsUIntOrAny();

            return GumpCommands.GumpExists( id ) ? 1 : 0;
        }

        private static double FindTypeExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 1 )
            {
                UOC.SystemMessage( "Usage: findtype (serial) [color] [source] [amount] [range]" );
                return 0;
            }

            int id = args[0].AsInt();
            //TODO
            int hue = args.Length > 1 ? args[1].AsIntOrAny() : -1;
            object source = args.Length > 2 ? (object) args[2].ResolveSerial( true ) : null;

            if ( source != null && source.ToString().Equals( "world", StringComparison.CurrentCultureIgnoreCase ) )
            {
                source = null;
            }

            if ( source != null && int.Parse( source.ToString() ) == -1 )
            {
                source = null;
            }

            //TODO
            // ReSharper disable once UnusedVariable
            int amount = args.Length > 3 ? args[3].AsIntOrAny() : -1;
            int range = args.Length > 4 ? args[4].AsIntOrAny() : -1;

            return ObjectCommands.FindType( id, range, source, hue ) ? 1 : 0;
        }

        [SuppressMessage( "ReSharper", "UnusedVariable" )]
        private static double FindObjectExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 1 )
            {
                UOC.SystemMessage( "Usage: findobject (serial) [color] [source] [amount] [range]" );
                return 0;
            }

            int serial = args[0].ResolveSerial();
            //TODO
            int hue = args.Length > 1 ? args[1].AsIntOrAny() : -1;
            object source = args.Length > 2 ? (object) args[2].ResolveSerial() : null;

            if ( source != null && int.Parse( source.ToString() ) == -1 )
            {
                source = null;
            }

            //TODO
            int amount = args.Length > 3 ? args[3].AsIntOrAny() : -1;
            int range = args.Length > 4 ? args[4].AsIntOrAny() : -1;

            return ObjectCommands.FindObject( serial, range, source ) ? 1 : 0;
        }

        private static double FindLayerExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: findlayer 'serial/alias' layer" );
                return 0;
            }

            int serial = args[0].ResolveSerial();
            string layer = ( (Layer) args[1].AsInt() ).ToString();

            return ActionCommands.FindLayer( layer, serial ) ? 1 : 0;
        }

        private static double DistanceExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: distance 'serial/alias'" );
                return 0;
            }

            int serial = args[0].ResolveSerial();

            return EntityCommands.Distance( serial );
        }

        private static double CountTypeGroundExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 3 )
            {
                UOC.SystemMessage( "Usage: counttypeground (graphic) (color) (range)" );
                return 0;
            }

            int id = args[0].AsInt();
            int hue = args[1].AsIntOrAny();
            int range = args[2].AsIntOrAny();

            return ObjectCommands.CountTypeGround( id, hue, range );
        }

        private static double CountTypeExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 3 )
            {
                UOC.SystemMessage( "Usage: counttype (graphic) (color) (source)" );
                return 0;
            }

            int id = args[0].AsInt();
            int hue = args[1].AsIntOrAny();
            int serial = args[2].ResolveSerial();

            return ObjectCommands.CountType( id, serial, hue );
        }

        private static double BuffExistsExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: buffexists 'buffName'" );
                return 0;
            }

            return EntityCommands.BuffExists( args[0].AsString() ) ? 1 : 0;
        }

        private static double ContentsExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: contents 'container'" );
                return 0;
            }

            return ActionCommands.Contents( args[0].ResolveSerial() );
        }

        private static double InListExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length < 2 )
            {
                UOC.SystemMessage( "Usage: inlist ('list name') ('element value')" );
                return 0;
            }

            string listName = args[0].AsString();
            int val = args[1].AsInt();

            return ListCommands.InList( listName, val ) ? 1 : 0;
        }

        private static double ListLengthExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: list ('list name')" );
                return 0;
            }

            string listName = args[0].AsString();

            return ListCommands.List( listName );
        }

        private static double ListExistsExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: listexists ('list name')" );
                return 0;
            }

            string listName = args[0].AsString();

            return ListCommands.ListExists( listName ) ? 1 : 0;
        }

        private static double InJournalExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: injournal ('text') ['author'/'system']" );
                return 0;
            }

            string text = args[0].AsString();
            string author = args.Length > 1 ? args[1].AsString() : string.Empty;

            return JournalCommands.InJournal( text, author ) ? 1 : 0;
        }

        private static double SkillExpression( string expression, Argument[] args, bool quiet )
        {
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: skill 'skillName'" );
                return 0;
            }

            return SkillCommands.Skill( args[0].AsString() );
        }

        private static double FindAliasExpression( string expression, Argument[] args, bool quiet )
        {
            // ReSharper disable once InvertIf
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: findalias 'alias'" );
                return 0;
            }

            return AliasCommands.FindAlias( args[0].AsString() ) ? 1 : 0;
        }

        private static double NotImplementedExpression( string expression, Argument[] args, bool quiet )
        {
            UOC.SystemMessage( string.Format( Strings.Command___0___currently_not_implemented___, expression ) );

            return 0;
        }

        private static double UnsupportedComparisonToString( string expression, Argument[] args, bool quiet )
        {
            UOC.SystemMessage( $"{expression}: UOScript doesn't currently support comparison to string." );

            return 0;
        }
    }
}