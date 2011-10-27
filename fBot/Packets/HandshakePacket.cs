using System;

namespace fBot {
    sealed class HandshakePacket : Packet {
        public const int CurrentProtocolVersion = 7;
        public byte ProtocolVersion { get; set; }
        public string ServerName { get; set; }
        public string MOTD { get; set; }
        public byte UnusedByte { get; set; }


        public HandshakePacket()
            : base( OpCode.Handshake ) { }

        public HandshakePacket( byte[] rawData )
            : base( OpCode.Handshake, rawData ) {
        }

        public HandshakePacket( string serverName, string motd )
            : base( OpCode.Handshake ) {
            ProtocolVersion = CurrentProtocolVersion;
            ServerName = serverName;
            MOTD = motd;
            UnusedByte = 0;
        }

        public HandshakePacket( HandshakePacket other )
            : base( OpCode.Handshake ) {
            ProtocolVersion = other.ProtocolVersion;
            ServerName = other.ServerName;
            MOTD = other.MOTD;
            UnusedByte = other.UnusedByte;
        }


        public override object Clone() {
            return new HandshakePacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            ProtocolVersion = data[1];
            ServerName = GetString( data, 2 );
            MOTD = GetString( data, 66 );
            UnusedByte = data[130];
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = ProtocolVersion;
            SetString( data, 2, ServerName );
            SetString( data, 66, MOTD );
            data[130] = UnusedByte;
        }

        public override string ToString() {
            return String.Format( "{0}(\"{1}\",\"{2}\")", OpCode, ServerName, MOTD );
        }
    }
}