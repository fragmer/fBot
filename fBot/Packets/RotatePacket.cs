using System;

namespace fBot {
    sealed class RotatePacket : Packet {
        public byte EntityId { get; set; }
        public Position EntityPosition { get; set; }


        public RotatePacket()
            : base( OpCode.Rotate ) { }

        public RotatePacket( byte[] rawData )
            : base( OpCode.Rotate, rawData ) {
        }

        public RotatePacket( byte entityId, Position entityPosition )
            : base( OpCode.Rotate ) {
            EntityId = entityId;
            EntityPosition = entityPosition;
        }

        public RotatePacket( byte entityId, byte rotation, byte look )
            : base( OpCode.Rotate ) {
            EntityId = entityId;
            EntityPosition = new Position {
                R = rotation,
                L = look
            };
        }

        public RotatePacket( RotatePacket other )
            : base( OpCode.Rotate ) {
            EntityId = other.EntityId;
            EntityPosition = other.EntityPosition;
        }


        public override object Clone() {
            return new RotatePacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            EntityId = data[1];
            EntityPosition = new Position {
                X = 0,
                Y = 0,
                Z = 0,
                R = data[2],
                L = data[3]
            };
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = EntityId;
            data[2] = EntityPosition.R;
            data[3] = EntityPosition.L;
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, EntityId, EntityPosition );
        }
    }
}