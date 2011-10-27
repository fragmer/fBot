using System;
using System.Net;
using System.Text;

namespace fBot {
    class Packet : ICloneable {
        protected Packet( OpCode opCode ) {
            if( !Enum.IsDefined( typeof( OpCode ), OpCode ) ) {
                throw new ArgumentOutOfRangeException( "opCode" );
            }
            OpCode = opCode;
        }

        protected Packet( OpCode opCode, byte[] rawData ) {
            OpCode = opCode;
            FromBytes( rawData );
        }

        public OpCode OpCode { get; protected set; }

        public byte[] Bytes {
            get {
                byte[] data = new byte[OpCode.GetPacketSize()];
                data[0] = (byte)OpCode;
                ToBytes( data );
                return data;
            }
            set {
                if( value == null ) {
                    throw new ArgumentNullException();
                }
                if( value.Length != OpCode.GetPacketSize() ) {
                    throw new ArgumentException( "Invalid packet size" );
                }
                if( value[0] != (byte)OpCode ) {
                    throw new ArgumentException( "Wrong opcode" );
                }
                FromBytes( value );
            }
        }

        protected virtual void FromBytes( byte[] data ) { }

        protected virtual void ToBytes( byte[] data ) { }


        protected static string GetString( byte[] data, int offset ) {
            return Encoding.ASCII.GetString( data, offset, 64 ).Trim();
        }

        protected static short GetShort( byte[] data, int offset ) {
            return IPAddress.HostToNetworkOrder( BitConverter.ToInt16( data, offset ) );
        }

        protected static Position GetPosition( byte[] data, int offset ) {
            return new Position {
                X = GetShort( data, offset ),
                Y = GetShort( data, offset + 4 ),
                Z = GetShort( data, offset + 2 ),
                R = data[offset + 6],
                L = data[offset + 7]
            };
        }

        protected static void SetShort( byte[] data, int offset, int number ) {
            data[offset] = (byte)((number & 0xff00) >> 8);
            data[offset + 1] = (byte)(number & 0x00ff);
        }

        protected static void SetString( byte[] data, int offset, string str ) {
            Encoding.ASCII.GetBytes( str.PadRight( 64 ), 0, 64, data, offset );
        }

        protected static void SetPosition( byte[] data, int offset, Position position ) {
            SetShort( data, offset, position.X );
            SetShort( data, offset + 4, position.Y );
            SetShort( data, offset + 2, position.Z );
            data[6] = position.R;
            data[7] = position.L;
        }


        public override string ToString() {
            return OpCode.ToString();
        }


        public virtual object Clone() {
            return new Packet( OpCode );
        }
    }
}