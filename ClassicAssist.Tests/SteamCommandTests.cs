using System;
using System.Threading;
using Assistant;
using ClassicAssist.Data.Macros;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.Data.Macros.Steam;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network.PacketFilter;
using ClassicAssist.UO.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicAssist.Tests
{
    [TestClass]
    public class SteamCommandTests
    {
        private SteamEngineInvoker _steamInvoker;

        [TestInitialize]
        public void Initialize()
        {
            Aliases.Register();
            Commands.Register();
            _steamInvoker = SteamEngineInvoker.GetInstance();

            Engine.Player = new PlayerMobile( 0x01 );
            Item backpack = new Item( 0x40000000 ) { Container = new ItemCollection( 0x40000000 ) };
            Engine.Player.SetLayer( Layer.Backpack, backpack.Serial );
            Engine.Items.Add( backpack );
        }

        [TestCleanup]
        public void Cleanup()
        {
            Engine.Player = null;
            Engine.Items.Clear();
            Engine.Mobiles.Clear();
            Engine.PacketWaitEntries = null;
        }

        [TestMethod]
        public void AttackAliasTest()
        {
            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( d => d[0] == 0x05, true );

            _steamInvoker.Execute( CreateMacro( "attack 'self'" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );
        }

        [TestMethod]
        public void AttackSerialTest()
        {
            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( d => d[0] == 0x05, true );

            _steamInvoker.Execute( CreateMacro( "attack 0x01" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );
        }

        [TestMethod]
        public void BandageSelfTest()
        {
            Item bandage = new Item( 0x40000001, 0x40000000 ) { ID = 0xe21 };
            Engine.Player.Backpack.Container.Add( bandage );

            Engine.PacketWaitEntries = new PacketWaitEntries();

            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( p => p[0] == 0x06, true );

            _steamInvoker.Execute( CreateMacro( "bandageself" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );

            Engine.Player.Backpack.Container.Clear();
        }

        [TestMethod]
        public void CastTest()
        {
            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( p => p[0] == 0xBF && p[4] == 0x1C, true );

            _steamInvoker.Execute( CreateMacro( "cast 'fireball'" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );
        }

        [TestMethod]
        public void ClearHandsTest()
        {
            Item onehanded = new Item(0x40000002  );
            Engine.Items.Add( onehanded );
            Engine.Player.SetLayer( Layer.OneHanded, onehanded.Serial );

            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( p => p[ 0 ] == 0x07 || p[0] == 0x08, true );

            _steamInvoker.Execute( CreateMacro( "clearhands 'left'" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );

            Engine.Items.Remove( onehanded );
            Engine.Player.SetLayer( Layer.OneHanded, 0 );
        }

        [TestMethod]
        public void ClickObjectTest()
        {
            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( p => p[ 0 ] == 0x09, true );

            _steamInvoker.Execute( CreateMacro( "clickobject 'backpack'" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );
        }

        [TestMethod]
        public void CreateListTest()
        {
            _steamInvoker.Execute( CreateMacro( "createlist 'test'" ) );

            Assert.IsTrue( ListCommands.ListExists( "test" ) );

            ListCommands.RemoveList( "test" );
        }

        [TestMethod]
        public void EquipItemTest()
        {
            Item onehanded = new Item( 0x40000002, Engine.Player.Backpack.Serial );
            Engine.Player.Backpack.Container.Add( onehanded );

            ExpectOutgoingPacket expect = new ExpectOutgoingPacket( p => p[ 0 ] == 0x07 || p[ 0 ] == 0x13, true );

            _steamInvoker.Execute( CreateMacro( "equipitem 0x40000002 1" ) );

            bool result = expect.WaitOne( 5000 );

            Assert.IsTrue( result );

            Engine.Items.Remove( onehanded );
            Engine.Player.SetLayer( Layer.OneHanded, 0 );
        }

        private static MacroEntry CreateMacro( string text )
        {
            return new MacroEntry { Name = "Test", Macro = text };
        }

        public class ExpectOutgoingPacket
        {
            private readonly AutoResetEvent _are = new AutoResetEvent( false );
            private readonly bool _failOnOther;
            private readonly Func<byte[], bool> _predicate;

            public ExpectOutgoingPacket( Func<byte[], bool> predicate, bool failOnOther )
            {
                _predicate = predicate;
                _failOnOther = failOnOther;

                Engine.InternalPacketSentEvent += OnSendPacket;
            }

            private void OnSendPacket( byte[] data, int length )
            {
                if ( _predicate.Invoke( data ) )
                {
                    _are.Set();
                }
                else
                {
                    if ( _failOnOther )
                    {
                        Assert.Fail($"Unexpected packet 0x{data[0]:X}");
                    }
                }
            }

            public bool WaitOne( int milliseconds )
            {
                bool result = _are.WaitOne( milliseconds );

                Engine.InternalPacketSentEvent -= OnSendPacket;

                return result;
            }
        }
    }
}