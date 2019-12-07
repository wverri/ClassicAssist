using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.UO.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicAssist.Tests.MacroCommands
{
    [TestClass]
    public class ObjectCommandTests
    {
        [TestMethod]
        public void WillFindTypeGround()
        {
            Engine.Items.Add( new Item( 0x40000001 ) { ID = 0xdda } );

            bool result = ObjectCommands.FindType( 0xdda );

            Assert.IsTrue( result );

            int val = AliasCommands.GetAlias( "found" );

            Assert.AreEqual( 0x40000001, val );

            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillFindTypeGroundMobile()
        {
            Engine.Mobiles.Add( new Mobile( 0x02 ) { ID = 0x190 } );

            bool result = ObjectCommands.FindType( 0x190 );

            Assert.IsTrue( result );

            int val = AliasCommands.GetAlias( "found" );

            Assert.AreEqual( 0x02, val );

            Engine.Mobiles.Clear();
        }

        [TestMethod]
        public void WillFindTypeSubcontainer()
        {
            Item container = new Item( 1 )
            {
                Container = new ItemCollection( 1 ) { new Item( 0x02 ) { ID = 0xdda, Owner = 1 } }
            };

            Engine.Items.Add( container );

            AliasCommands.SetAlias( "cont", 1 );

            bool result = ObjectCommands.FindType( 0xdda, -1, "cont" );

            Assert.IsTrue( result );

            int val = AliasCommands.GetAlias( "found" );

            Assert.AreEqual( 0x02, val );

            Engine.Items.Clear();
        }

        [TestMethod]
        public void WillFindTypeInRange()
        {
            Engine.Player = new PlayerMobile( 1 );
            Engine.Mobiles.Add( new Mobile( 0x02 ) { ID = 0x190, X = 5, Y = 5 } );

            bool result = ObjectCommands.FindType( 0x190, 8 );

            Assert.IsTrue( result );

            Engine.Mobiles.Clear();
            Engine.Player = null;
        }

        [TestMethod]
        public void WontFindTypeOutOfRange()
        {
            Engine.Player = new PlayerMobile( 1 );
            Engine.Mobiles.Add( new Mobile( 0x02 ) { ID = 0x190, X = 5, Y = 5 } );

            bool result = ObjectCommands.FindType( 0x190, 1 );

            Assert.IsFalse( result );

            Engine.Mobiles.Clear();
            Engine.Player = null;
        }

        [TestMethod]
        public void WillIgnoreObject()
        {
            Engine.Mobiles.Add( new Mobile( 0x02 ) { ID = 0x190, X = 5, Y = 5 } );

            bool result = ObjectCommands.FindType( 0x190 );

            Assert.IsTrue( result );

            ObjectCommands.IgnoreObject( "found" );

            result = ObjectCommands.FindType( 0x190 );

            Assert.IsFalse( result );

            Engine.Mobiles.Clear();
            ObjectCommands.ClearIgnoreList();
        }
    }
}