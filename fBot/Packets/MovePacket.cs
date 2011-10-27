using System;

namespace fBot {
    sealed class MovePacket : Packet {
        public byte EntityId { get; set; }
        public Position EntityPositionDelta { get; set; }


        public MovePacket()
            : base( OpCode.Move ) { }

        public MovePacket( byte[] rawData )
            : base( OpCode.Move, rawData ) {
        }

        public MovePacket( byte entityId, Position entityPositionDelta )
            : base( OpCode.Move ) {
            EntityId = entityId;
            EntityPositionDelta = entityPositionDelta;
        }

        public MovePacket( byte entityId, int deltaX, int deltaY, int deltaZ )
            : base( OpCode.Move ) {
            EntityId = entityId;
            EntityPositionDelta = new Position( deltaX, deltaY, deltaZ );
        }

        public MovePacket( MovePacket other )
            : base( OpCode.Move ) {
            EntityId = other.EntityId;
            EntityPositionDelta = other.EntityPositionDelta;
        }


        public override object Clone() {
            return new MovePacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            EntityId = data[1];
            EntityPositionDelta = new Position {
                X = data[2],
                Y = data[4],
                Z = data[3],
                R = 0,
                L = 0
            };
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = EntityId;
            data[2] = (byte)EntityPositionDelta.X;
            data[4] = (byte)EntityPositionDelta.Y;
            data[3] = (byte)EntityPositionDelta.Z;
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, EntityId, EntityPositionDelta );
        }
    }
}