namespace ClassicAssist.UO.Network
{
    public static class OutgoingPacketHandlers
    {
        private static PacketHandler[] _handlers;
        private static PacketHandler[] _extendedHandlers;

        public static void Initialize()
        {
            _handlers = new PacketHandler[0x100];
            _extendedHandlers = new PacketHandler[0x100];
        }

        private static void Register(int packetId, int length, OnPacketReceive onReceive)
        {
            _handlers[packetId] = new PacketHandler(packetId, length, onReceive);
        }

        private static void RegisterExtended(int packetId, int length, OnPacketReceive onReceive)
        {
            _handlers[packetId] = new PacketHandler(packetId, length, onReceive);
        }

        internal static PacketHandler GetHandler(int packetId)
        {
            return _handlers[packetId];
        }

        private static PacketHandler GetExtendedHandler(int packetId)
        {
            return _handlers[packetId];
        }
    }
}