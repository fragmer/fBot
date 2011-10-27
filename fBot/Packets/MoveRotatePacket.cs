using System;

namespace fBot {
    sealed class MoveRotatePacket : Packet {
        public byte EntityId { get; set; }
        public Position EntityPositionDelta { get; set; }


        public MoveRotatePacket()
            : base( OpCode.MoveRotate ) { }

        public MoveRotatePacket( byte[] rawData )
            : base( OpCode.MoveRotate, rawData ) {
        }

        public MoveRotatePacket( byte entityId, Position entityPositionDelta )
            : base( OpCode.MoveRotate ) {
            EntityId = entityId;
            EntityPositionDelta = entityPositionDelta;
        }

        public MoveRotatePacket( MoveRotatePacket other )
            : base( OpCode.MoveRotate ) {
            EntityId = other.EntityId;
            EntityPositionDelta = other.EntityPositionDelta;
        }


        public override object Clone() {
            return new MoveRotatePacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            EntityId = data[1];
            EntityPositionDelta = new Position {
                X = data[2],
                Y = data[4],
                Z = data[3],
                R = data[5],
                L = data[6]
            };
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = EntityId;
            data[2] = (byte)EntityPositionDelta.X;
            data[4] = (byte)EntityPositionDelta.Y;
            data[3] = (byte)EntityPositionDelta.Z;
            data[5] = EntityPositionDelta.R;
            data[6] = EntityPositionDelta.L;
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, EntityId, EntityPositionDelta );
        }
    }
}