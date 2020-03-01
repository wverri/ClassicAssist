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
using System.Globalization;
using ClassicAssist.Data.Macros.Commands;
using UOScript;

namespace ClassicAssist.Data.Macros.Steam
{
    public static class ArgumentExtensionMethods
    {
        public static int ResolveSerial( this Argument arg, bool quiet = false )
        {
            int val;

            if ( arg.AsString().StartsWith( "0x" ) )
            {
                if ( int.TryParse( arg.AsString().Substring( 2 ), NumberStyles.HexNumber, Interpreter.Culture,
                    out val ) )
                {
                    return val;
                }
            }
            else if ( int.TryParse( arg.AsString(), out val ) )
            {
                return val;
            }
            else
            {
                val = AliasCommands.ResolveSerial( arg.AsString(), quiet );
            }

            return val;
        }

        public static int AsIntOrAny( this Argument arg )
        {
            return arg.AsString().Equals( "Any", StringComparison.CurrentCultureIgnoreCase ) ? -1 : arg.AsInt();
        }

        public static uint AsUIntOrAny( this Argument arg )
        {
            return arg.AsString().Equals( "Any", StringComparison.CurrentCultureIgnoreCase ) ? 0 : arg.AsUInt();
        }
    }
}