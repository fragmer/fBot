using System;

namespace fBot {
    sealed class RemoveEntityPacket : Packet {
        public byte EntityId { get; set; }


        public RemoveEntityPacket()
            : base( OpCode.RemoveEntity ) { }

        public RemoveEntityPacket( byte[] rawData )
            : base( OpCode.RemoveEntity, rawData ) {
            EntityId = rawData[0];
        }

        public RemoveEntityPacket( byte entityId )
            : base( OpCode.RemoveEntity ) {
            EntityId = entityId;
        }

        public RemoveEntityPacket( RemoveEntityPacket other )
            : base( OpCode.RemoveEntity ) {
            EntityId = other.EntityId;
        }


        public override object Clone() {
            return new RemoveEntityPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            EntityId = data[1];
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = EntityId;
        }

        public override string ToString() {
            return String.Format( "{0}({1})", OpCode, EntityId );
        }
    }
}