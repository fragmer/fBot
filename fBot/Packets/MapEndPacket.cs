using System;

namespace fBot {
    sealed class MapEndPacket : Packet {
        public Vector3I MapDimensions { get; set; }


        public MapEndPacket()
            : base( OpCode.MapEnd ) { }

        public MapEndPacket( byte[] rawData )
            : base( OpCode.MapEnd, rawData ) {
        }

        public MapEndPacket( Vector3I mapDimensions )
            : base( OpCode.MapEnd ) {
            MapDimensions = mapDimensions;
        }

        public MapEndPacket( int width, int length, int height )
            : base( OpCode.MapEnd ) {
            MapDimensions = new Vector3I( width, length, height );
        }

        public MapEndPacket( MapEndPacket other )
            : base( OpCode.MapEnd ) {
            MapDimensions = other.MapDimensions;
        }


        public override object Clone() {
            return new MapEndPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            MapDimensions = new Vector3I {
                X = GetShort( data, 1 ),
                Y = GetShort( data, 5 ),
                Z = GetShort( data, 3 )
            };
        }

        protected override void ToBytes( byte[] data ) {
            SetShort( data, 1, MapDimensions.X );
            SetShort( data, 5, MapDimensions.Y );
            SetShort( data, 3, MapDimensions.Z );
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2},{3})",
                                  OpCode,
                                  MapDimensions.X,
                                  MapDimensions.Y,
                                  MapDimensions.Z );
        }
    }
}