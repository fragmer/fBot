using System;

namespace fBot {
    sealed class TeleportPacket : Packet {
        public const byte SelfId = 255;
        public byte EntityId { get; set; }
        public Position EntityPosition { get; set; }


        public TeleportPacket()
            : base( OpCode.Teleport ) { }

        public TeleportPacket( byte[] rawData )
            : base( OpCode.Teleport, rawData ) { }

        public TeleportPacket( Position position ) :
            base( OpCode.Teleport ) {
            EntityId = SelfId;
            EntityPosition = position;
        }

        public TeleportPacket(  int x, int y, int z ) :
            base( OpCode.Teleport ) {
            EntityId = SelfId;
            EntityPosition = new Position( x, y, z );
        }

        public TeleportPacket( int x, int y, int z, byte entityId ) :
            base( OpCode.Teleport ) {
            EntityId = entityId;
            EntityPosition = new Position( x, y, z );
        }

        public TeleportPacket( Position entityPosition, byte entityId ) :
            base( OpCode.Teleport ) {
            EntityId = entityId;
            EntityPosition = entityPosition;
        }

        public TeleportPacket( TeleportPacket other )
            : base( OpCode.Teleport ) {
            EntityId = other.EntityId;
            EntityPosition = other.EntityPosition;
        }


        public override object Clone() {
            return new TeleportPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            EntityId = data[1];
            EntityPosition = GetPosition( data, 2 );
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = EntityId;
            SetPosition( data, 2, EntityPosition );
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, EntityId, EntityPosition );
        }
    }
}