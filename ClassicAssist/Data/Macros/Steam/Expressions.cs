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

using Assistant;
using ClassicAssist.Data.Macros.Commands;
using UOScript;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class Expressions
    {
        public static void Register()
        {
            Interpreter.RegisterExpressionHandler( "findalias", FindAlias );
            Interpreter.RegisterExpressionHandler( "contents", DummyExpression );
            Interpreter.RegisterExpressionHandler( "inregion", DummyExpression );
            Interpreter.RegisterExpressionHandler( "skill", SkillExpression );
            Interpreter.RegisterExpressionHandler( "findobject", DummyExpression );
            Interpreter.RegisterExpressionHandler( "distance", DummyExpression );
            Interpreter.RegisterExpressionHandler( "inrange", DummyExpression );
            Interpreter.RegisterExpressionHandler( "buffexists", DummyExpression );
            Interpreter.RegisterExpressionHandler( "property", DummyExpression );
            Interpreter.RegisterExpressionHandler( "findtype", DummyExpression );
            Interpreter.RegisterExpressionHandler( "findlayer", DummyExpression );
            Interpreter.RegisterExpressionHandler( "skillstate", DummyExpression );
            Interpreter.RegisterExpressionHandler( "counttype", DummyExpression );
            Interpreter.RegisterExpressionHandler( "counttypeground", DummyExpression );
            Interpreter.RegisterExpressionHandler( "findwand", DummyExpression );
            Interpreter.RegisterExpressionHandler( "inparty", DummyExpression );
            Interpreter.RegisterExpressionHandler( "infriendslist", DummyExpression );
            Interpreter.RegisterExpressionHandler( "war", DummyExpression );
            Interpreter.RegisterExpressionHandler( "ingump", DummyExpression );
            Interpreter.RegisterExpressionHandler( "gumpexists", DummyExpression );
            Interpreter.RegisterExpressionHandler( "injournal", InJournal );
            Interpreter.RegisterExpressionHandler( "listexists", ListExists );
            Interpreter.RegisterExpressionHandler( "list", ListLength );
            Interpreter.RegisterExpressionHandler( "inlist", InList );
            Interpreter.RegisterExpressionHandler( "timer", DummyExpression );
            Interpreter.RegisterExpressionHandler( "timerexists", DummyExpression );

            // Player Attributes
            Interpreter.RegisterExpressionHandler( "mana", Mana );
            Interpreter.RegisterExpressionHandler( "x", X );
            Interpreter.RegisterExpressionHandler( "y", Y );
            Interpreter.RegisterExpressionHandler( "z", Z );
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
            return 0;
        }

        private static double InList( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }

        private static double ListLength( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }

        private static double ListExists( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }

        private static double InJournal( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }

        private static double SkillExpression( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }

        private static double FindAlias( string expression, Argument[] args, bool quiet )
        {
            // ReSharper disable once InvertIf
            if ( args.Length == 0 )
            {
                UOC.SystemMessage( "Usage: sysmsg 'text' [hue]" );
                return 0;
            }

            return AliasCommands.FindAlias( args[0].AsString() ) ? 1 : 0;
        }

        private static double DummyExpression( string expression, Argument[] args, bool quiet )
        {
            return 0;
        }
    }
}