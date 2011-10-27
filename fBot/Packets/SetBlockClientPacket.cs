using System;

namespace fBot {
    sealed class SetBlockClientPacket : Packet {
        public Vector3I Coordinates { get; set; }
        public Block Block { get; set; }
        public ClickAction ClickAction { get; set; }


        public SetBlockClientPacket()
            : base( OpCode.SetBlockClient ) { }

        public SetBlockClientPacket( byte[] rawData )
            : base( OpCode.SetBlockClient, rawData ) { }

        public SetBlockClientPacket( Block block, ClickAction clickAction, Vector3I coordinates )
            : base( OpCode.SetBlockClient ) {
            Coordinates = coordinates;
            Block = block;
            ClickAction = clickAction;
        }

        public SetBlockClientPacket( SetBlockServerPacket packet, ClickAction clickAction )
            : base( OpCode.SetBlockClient ) {
            Coordinates = packet.Coordinates;
            Block = packet.Block;
            ClickAction = clickAction;
        }

        public SetBlockClientPacket( Block block, ClickAction clickAction, int x, int y, int z )
            : base( OpCode.SetBlockClient ) {
            Coordinates = new Vector3I( x, y, z );
            Block = block;
            ClickAction = clickAction;
        }

        public SetBlockClientPacket( SetBlockClientPacket other )
            : base( OpCode.SetBlockClient ) {
            Coordinates = other.Coordinates;
            Block = other.Block;
            ClickAction = other.ClickAction;
        }


        public override object Clone() {
            return new SetBlockClientPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            Coordinates = new Vector3I {
                X = GetShort( data, 1 ),
                Y = GetShort( data, 5 ),
                Z = GetShort( data, 3 )
            };
            Block = (Block)data[7];
            ClickAction = (ClickAction)data[8];
        }

        protected override void ToBytes( byte[] data ) {
            SetShort( data, 1, Coordinates.X );
            SetShort( data, 5, Coordinates.Y );
            SetShort( data, 3, Coordinates.Z );
            data[7] = (byte)Block;
            data[8] = (byte)ClickAction;
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, Coordinates, Block );
        }
    }
}