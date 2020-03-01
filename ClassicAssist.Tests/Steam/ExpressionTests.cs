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

using System.Diagnostics;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Data.Friends;
using ClassicAssist.Data.Macros;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicAssist.Tests.Steam
{
    [TestClass]
    public class ExpressionTests
    {
        private SteamInvoker _steamInvoker;

        [TestInitialize]
        public void Initialize()
        {
            _steamInvoker = SteamInvoker.GetInstance();
            _steamInvoker.ExceptionEvent += e =>
            {
                Debug.Write( e.ToString() );
                Assert.Fail( e.Message );
            };
        }

        [TestMethod]
        public void WillCountTypeGround()
        {
            Engine.Player = new PlayerMobile( 0x1 );
            Engine.Items.Add( new Item( 0x40000001, 0 ) { ID = 0x1f13 } );

            MacroEntry macro = new MacroEntry( MacroType.Steam )
            {
                Macro = SetAliasOnExpression( "if counttypeground 0x1f13 'any' 10 > 0" )
            };

            ExecuteAndWait( macro );

            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            Engine.Player = null;
            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillFindDistance()
        {
            Engine.Player = new PlayerMobile( 0x1 );
            Engine.Items.Add( new Item( 0x40000001, 0 ) { ID = 0x1f13, X = 4 } );

            MacroEntry macro =
                new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if distance 0x40000001 == 4" ) };

            ExecuteAndWait( macro );

            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            Engine.Player = null;
            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillFindLayer()
        {
            Engine.Player = new PlayerMobile( 0x1 );
            Item item = new Item( 0x40000001, 0 ) { ID = 0x1f13, Layer = Layer.Backpack };
            Engine.Items.Add( item );
            Engine.Player.Equipment.Add( item );
            Engine.Player.SetLayer( Layer.Backpack, item.Serial );

            MacroEntry macro =
                new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if findlayer 'self' 21" ) };

            ExecuteAndWait( macro );

            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            Engine.Player = null;
            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillFindObject()
        {
            Engine.Player = new PlayerMobile( 0x1 );
            Engine.Items.Add( new Item( 0x40000001, 0 ) { ID = 0x1f13, X = 4 } );

            // no arguments
            MacroEntry macro =
                new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if findobject 0x40000001" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            // range
            macro = new MacroEntry( MacroType.Steam )
            {
                Macro = SetAliasOnExpression( "if findobject 0x40000001 0 'any' 'any' 3" )
            };
            ExecuteAndWait( macro );
            Assert.AreEqual( -1, AliasCommands.GetAlias( "f" ) );
            macro = new MacroEntry( MacroType.Steam )
            {
                Macro = SetAliasOnExpression( "if findobject 0x40000001 0 'any' 'any' 6" )
            };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            Engine.Player = null;
            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillFindType()
        {
            Engine.Player = new PlayerMobile( 0x1 );
            Engine.Items.Add( new Item( 0x40000001, 0 ) { ID = 0x1f13, X = 4 } );

            // no arguments
            MacroEntry macro =
                new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if findtype 0x1f13" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            // range
            macro = new MacroEntry( MacroType.Steam )
            {
                Macro = SetAliasOnExpression( "if findtype 0x1f13 0 'any' 'any' 3" )
            };
            ExecuteAndWait( macro );
            Assert.AreEqual( -1, AliasCommands.GetAlias( "f" ) );
            macro = new MacroEntry( MacroType.Steam )
            {
                Macro = SetAliasOnExpression( "if findtype 0x1f13 0 'any' 'any' 6" )
            };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            Engine.Player = null;
            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillGetInFriendList()
        {
            Engine.Player = new PlayerMobile( 0x1 );
            Mobile friend = new Mobile( 0x02 );
            Engine.Mobiles.Add( friend );

            Options.CurrentOptions.Friends.Add( new FriendEntry { Name = "Friend", Serial = friend.Serial } );

            MacroEntry macro =
                new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if infriendslist 2" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            macro = new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if infriendslist 3" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( -1, AliasCommands.GetAlias( "f" ) );

            Engine.Player = null;
            Engine.Mobiles.Clear();
        }

        [TestMethod]
        public void WillGetInJournal()
        {
            Engine.Journal.Write( new JournalEntry { Text = "Shmoo", Name = "Tony" } );

            MacroEntry macro =
                new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if injournal 'moo'" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            macro = new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if injournal 'moo' 'tony'" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            Engine.Journal.Clear();
        }

        [TestMethod]
        public void WillGetInList()
        {
            ListCommands.CreateList( "shmoo" );
            ListCommands.PushList( "shmoo", 1 );

            MacroEntry macro = new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if inlist 'shmoo' 1" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( 1, AliasCommands.GetAlias( "f" ) );

            macro = new MacroEntry( MacroType.Steam ) { Macro = SetAliasOnExpression( "if inlist 'shmoo' 0" ) };
            ExecuteAndWait( macro );
            Assert.AreEqual( -1, AliasCommands.GetAlias( "f" ) );

            ListCommands.RemoveList( "shmoo" );
        }

        private static string SetAliasOnExpression( string expression )
        {
            return $"{expression}\n" +
                   "\tsetalias 'f' 1\n" +
                   "else\n" +
                   "\tunsetalias 'f'\n" +
                   "endif\n";
        }

        public void ExecuteAndWait( MacroEntry macro, int timeout = 30000 )
        {
            _steamInvoker.Execute( macro );
            _steamInvoker.Thread.Join( timeout );
        }
    }
}