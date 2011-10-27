using System;

namespace fBot {
    sealed class SetBlockServerPacket : Packet {
        public Vector3I Coordinates { get; set; }
        public Block Block { get; set; }


        public SetBlockServerPacket()
            : base( OpCode.SetBlockServer ) { }

        public SetBlockServerPacket( byte[] rawData )
            : base( OpCode.SetBlockServer, rawData ) { }

        public SetBlockServerPacket( Block block, Vector3I coordinates )
            : base( OpCode.SetBlockServer ) {
            Coordinates = coordinates;
            Block = block;
        }

        public SetBlockServerPacket( Block block, int x, int y, int z )
            : base( OpCode.SetBlockServer ) {
            Coordinates = new Vector3I(x,y,z);
            Block = block;
        }

        public SetBlockServerPacket( SetBlockClientPacket packet )
            : base( OpCode.SetBlockServer ) {
            Coordinates = packet.Coordinates;
            Block = packet.Block;
        }

        public SetBlockServerPacket( SetBlockServerPacket other )
            : base( OpCode.SetBlockServer ) {
            Coordinates = other.Coordinates;
            Block = other.Block;
        }


        public override object Clone() {
            return new SetBlockServerPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            Coordinates = new Vector3I {
                X = GetShort( data, 1 ),
                Y = GetShort( data, 5 ),
                Z = GetShort( data, 3 )
            };
            Block = (Block)data[7];
        }

        protected override void ToBytes( byte[] data ) {
            SetShort( data, 1, Coordinates.X );
            SetShort( data, 5, Coordinates.Y );
            SetShort( data, 3, Coordinates.Z );
            data[7] = (byte)Block;
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, Coordinates, Block );
        }
    }
}