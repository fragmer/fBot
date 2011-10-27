using System.IO;
using System.Net;
using System.Text;

namespace fBot {
    sealed class PacketReader : BinaryReader {
        public PacketReader( Stream stream ) :
            base( stream ) { }

        public OpCode ReadOpCode() {
            return (OpCode)ReadByte();
        }

        public short ReadInt16BE() {
            return IPAddress.NetworkToHostOrder( ReadInt16() );
        }

        public int ReadInt32BE() {
            return IPAddress.NetworkToHostOrder( ReadInt32() );
        }

        public string ReadMCString() {
            return Encoding.ASCII.GetString( ReadBytes( 64 ) ).Trim();
        }

        public Packet ReadPacket() {
            OpCode opCode = (OpCode)ReadByte();
            int packetSize = opCode.GetPacketSize();

            byte[] rawData = new byte[packetSize];
            rawData[0] = (byte)opCode;

            int read = 1;
            do {
                read += Read( rawData, read, packetSize - read );
            } while( read < packetSize );

            switch( opCode ) {
                case OpCode.Handshake:
                    return new HandshakePacket( rawData );
                case OpCode.Ping:
                    return new PingPacket();
                case OpCode.MapBegin:
                    return new MapBeginPacket();
                case OpCode.MapChunk:
                    return new MapChunkPacket( rawData );
                case OpCode.MapEnd:
                    return new MapEndPacket( rawData );
                case OpCode.SetBlockClient:
                    return new SetBlockClientPacket( rawData );
                case OpCode.SetBlockServer:
                    return new SetBlockServerPacket( rawData );
                case OpCode.AddEntity:
                    return new AddEntityPacket( rawData );
                case OpCode.Teleport:
                    return new TeleportPacket( rawData );
                case OpCode.MoveRotate:
                    return new MoveRotatePacket( rawData );
                case OpCode.Move:
                    return new MovePacket( rawData );
                case OpCode.Rotate:
                    return new RotatePacket( rawData );
                case OpCode.RemoveEntity:
                    return new RemoveEntityPacket( rawData );
                case OpCode.Message:
                    return new MessagePacket( rawData );
                case OpCode.Kick:
                    return new KickPacket( rawData );
                case OpCode.SetPermission:
                    return new SetPermissionPacket( rawData );
                default:
                    throw new InvalidDataException();
            }
        }
    }
}