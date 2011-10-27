namespace fBot {
    sealed class PingPacket : Packet {
        public PingPacket() : base( OpCode.Ping ) { }
    }
}