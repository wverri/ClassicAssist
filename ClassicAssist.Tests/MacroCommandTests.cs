using Assistant;
using ClassicAssist.Data.Macros.Commands;
using ClassicAssist.UO.Data;
using ClassicAssist.UO.Network;
using ClassicAssist.UO.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassicAssist.Tests
{
    [TestClass]
    public class MacroCommandTests
    {
        private PlayerMobile _player;

        [TestInitialize]
        public void Initialize()
        {
            _player = new PlayerMobile( 0x01 );
            Engine.Player = _player;
        }

        [TestMethod]
        public void WillGetHitsNoParam()
        {
            _player.Hits = 100;

            int val = MobileCommands.Hits();

            Assert.AreEqual( 100, _player.Hits );

            _player.Hits = 0;
        }

        [TestMethod]
        public void WillGetHitsSerialParam()
        {
            Mobile mobile = new Mobile( 2 ) { Hits = 200 };

            Engine.Mobiles.Add( mobile );

            int val = MobileCommands.Hits(mobile.Serial);

            Assert.AreEqual( 200, val );

            Engine.Mobiles.Remove( mobile );
        }

        [TestMethod]
        public void WillGetHitsStringParam()
        {
            Mobile mobile = new Mobile(2) { Hits = 250 };

            Engine.Mobiles.Add(mobile);

            AliasCommands.SetAlias( "mobile", mobile.Serial );

            int val = MobileCommands.Hits("mobile");

            Assert.AreEqual(250, val);

            Engine.Mobiles.Remove(mobile);
        }

        [TestMethod]
        public void WillGetHiddenNoParam()
        {
            _player.Status |= MobileStatus.Hidden;
            _player.Status |= MobileStatus.Female;

            bool hidden = MobileCommands.Hidden();

            Assert.IsTrue( hidden );
        }

        [TestMethod]
        public void WillSetDefaultAliases()
        {
            AliasCommands.SetDefaultAliases();

            Assert.AreEqual( _player.Serial, (int)AliasCommands.GetAlias( "self" ) );

            AliasCommands.UnsetDefaultAliases();
        }

        [TestMethod]
        public void WillChangeLastAliasOnTargetSent()
        {
            _player.LastTargetSerial = 0x02;

            AliasCommands.SetDefaultAliases();

            Assert.AreEqual(0x02, (int)AliasCommands.GetAlias("last"));

            OutgoingPacketHandlers.Initialize();
            PacketHandler handler = OutgoingPacketHandlers.GetHandler( 0x6C );

            byte[] packet = {
                0x6C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xAA, 0xBB, 0xCC, 0xDD, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            handler?.OnReceive(new PacketReader( packet, packet.Length, true ));

            Assert.AreEqual((uint)0xAABBCCDD, (uint)AliasCommands.GetAlias("last"));

            AliasCommands.UnsetDefaultAliases();
        }
    }
}