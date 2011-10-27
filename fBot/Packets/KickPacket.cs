using System;

namespace fBot {
    sealed class KickPacket : Packet {
        public string Message { get; set; }


        public KickPacket()
            : base( OpCode.Kick ) { }

        public KickPacket( byte[] rawData )
            : base( OpCode.Kick, rawData ) {
        }

        public KickPacket( string message )
            : base( OpCode.Kick ) {
            Message = message;
        }

        public KickPacket( KickPacket other )
            : base( OpCode.Kick ) {
            Message = other.Message;
        }


        public override object Clone() {
            return new KickPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            Message = GetString( data, 1 );
        }

        protected override void ToBytes( byte[] data ) {
            SetString( data, 1, Message );
        }

        public override string ToString() {
            return String.Format( "{0}(\"{1}\")", OpCode, Message );
        }
    }
}