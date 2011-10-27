namespace fBot {
    sealed class MapBeginPacket : Packet {
        public MapBeginPacket() :
            base( OpCode.MapBegin ) { }
    }
}