using System;

namespace fBot {
    sealed class MapChunkPacket : Packet {
        public byte[] ChunkData { get; set; }
        public byte Progress { get; set; }


        public MapChunkPacket()
            : base( OpCode.MapChunk ) { }

        public MapChunkPacket( byte[] rawData )
            : base( OpCode.MapChunk, rawData ) {
        }

        public MapChunkPacket( byte[] chunkData, byte progress )
            : base( OpCode.MapChunk ) {
            ChunkData = chunkData;
            Progress = progress;
        }

        public MapChunkPacket( MapChunkPacket other )
            : base( OpCode.MapChunk ) {
            ChunkData = other.ChunkData;
            Progress = other.Progress;
        }


        public override object Clone() {
            return new MapChunkPacket( this );
        }

        protected override void FromBytes( byte[] rawData ) {
            int chunkSize = GetShort( rawData, 1 );
            ChunkData = new byte[chunkSize];
            Array.Copy( rawData, 3, ChunkData, 0, chunkSize );
            Progress = rawData[1027];
        }

        protected override void ToBytes( byte[] data ) {
            SetShort( data, 1, ChunkData.Length );
            Array.Copy( ChunkData, 0, data, 3, ChunkData.Length );
            Array.Clear( data, 3 + ChunkData.Length, 1024 - ChunkData.Length );
            data[1027] = Progress;
        }

        public override string ToString() {
            return String.Format( "{0}({1},{2})", OpCode, ChunkData.Length, Progress );
        }
    }
}