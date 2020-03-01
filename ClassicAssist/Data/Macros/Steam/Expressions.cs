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
            Interpreter.RegisterExpressionHandler( "counttype", CountTypeExpression );
            Interpreter.RegisterExpressionHandler( "counttypeground", CountTypeGroundExpression );
            Interpreter.RegisterExpressionHandler( "distance", DistanceExpression );
            Interpreter.RegisterExpressionHandler( "findalias", FindAliasExpression );
            Interpreter.RegisterExpressionHandler( "findlayer", FindLayerExpression );
            Interpreter.RegisterExpressionHandler( "findobject", FindObjectExpression );
            Interpreter.RegisterExpressionHandler( "findtype", FindTypeExpression );
            Interpreter.RegisterExpressionHandler( "findwand", NotImplementedExpression );
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
            Interpreter.RegisterExpressionHandler( "property", PropertyExpression );
            Interpreter.RegisterExpressionHandler( "skill", SkillExpression );
            Interpreter.RegisterExpressionHandler( "skillstate", SkillStateExpression );
            Interpreter.RegisterExpressionHandler( "timer", TimerExpression );
            Interpreter.RegisterExpressionHandler( "timerexists", TimerExistsExpression );
            Interpreter.RegisterExpressionHandler( "war", WarExpression );

            // Player Attributes
            Interpreter.RegisterExpressionHandler( "weight", Weight );
            Interpreter.RegisterExpressionHandler( "mana", Mana );
            Interpreter.RegisterExpressionHandler( "x", X );
            Interpreter.RegisterExpressionHandler( "y", Y );
            Interpreter.RegisterExpressionHandler( "z", Z );
        }

        private static double Weight( string expression, Argument[] args, bool quiet )
        {
            if ( Engine.Player != null )
            {
                return Engine.Player.Weight;
            }

            return 0;
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

        private static double Z( string expression, Argument[] args, bool quiet )
        {
            if ( Engine.Player != null )
            {
                return Engine.Player.Z;
            }

            return 0;
        }

        private static double Y( string expression, Argument[] args, bool quiet )
        {
            if ( Engine.Player != null )
            {
                return Engine.Player.Y;
            }

            return 0;
        }

        private static double X( string expression, Argument[] args, bool quiet )
        {
            if ( Engine.Player != null )
            {
                return Engine.Player.X;
            }

            return 0;
        }

        private static double Mana( string expression, Argument[] args, bool quiet )
        {
            if ( Engine.Player != null )
            {
                return Engine.Player.Mana;
            }

            return 0;
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

        private static double DummyExpression( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }

        private static double NotImplementedExpression( string expression, Argument[] args, bool quiet )
        {
            UOC.SystemMessage( Strings.Command_currently_not_implemented_ );

            return 0;
        }
    }
}