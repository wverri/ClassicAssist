using System.Collections.Generic;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class ListCommands
    {
        private static readonly Dictionary<string, List<int>> _lists = new Dictionary<string, List<int>>();

        public static void CreateList( string listName )
        {
            if ( ListExists( listName ) )
            {
                RemoveList( listName );
            }

            _lists.Add( listName, new List<int>() );
        }

        public static bool ListExists( string listName )
        {
            return _lists.ContainsKey( listName );
        }

        public static void PushList( string listName, int value )
        {
            if ( !ListExists( listName ) )
            {
                CreateList( listName );
            }

            _lists[listName].Add( value );
        }

        public static int[] GetList( string listName )
        {
            return _lists[listName].ToArray();
        }

        public static void RemoveList( string listName )
        {
            _lists.Remove( listName );
        }

        public static Dictionary<string, List<int>> GetAllLists()
        {
            return _lists;
        }
    }
}