/* Copyright (C) 2009 Matthew Geyer
 * 
 * This file is part of UO Machine.
 * 
 * UO Machine is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * UO Machine is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.Text;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.Misc
{
    internal class DisplayFormatAttribute : Attribute
    {
        private readonly Type _type;

        public DisplayFormatAttribute(Type type)
        {
            _type = type;
        }

        public string ToString(object value)
        {
            if (_type.IsEnum)
                return Enum.Parse(_type, value.ToString()).ToString();

            return typeof(IFormatProvider).IsAssignableFrom( _type ) ? string.Format((IFormatProvider)Activator.CreateInstance(_type), "{0}", value) : Convert.ChangeType(value, typeof(string)).ToString();
        }
    }

    internal class HexFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return this;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return $"0x{arg:x}";
        }
    }

    public class PropertyArrayFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat( Type formatType )
        {
            return this;
        }

        public string Format( string format, object arg, IFormatProvider formatProvider )
        {
            if ( arg == null )
            {
                return "null";
            }

            if ( !( arg is Property[] properties ) )
            {
                return arg.ToString();
            }

            StringBuilder sb = new StringBuilder();

            for ( int index = 0; index < properties.Length; index++ )
            {
                Property property = properties[index];

                if ( index != properties.Length - 1 )
                {
                    sb.AppendLine( property.Text );
                }
                else
                {
                    sb.Append( property.Text );
                }
            }

            return sb.ToString();

        }
    }
}