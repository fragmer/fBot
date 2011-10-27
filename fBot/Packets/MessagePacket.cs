using System;

namespace fBot {
    sealed class MessagePacket : Packet {
        public byte MessageByte { get; set; }
        public string Text { get; set; }


        public MessagePacket()
            : base( OpCode.Message ) { }

        public MessagePacket( byte[] rawData )
            : base( OpCode.Message, rawData ) {
        }

        public MessagePacket( string text )
            : base( OpCode.Message ) {
            Text = text;
        }

        public MessagePacket(  string text, byte messageByte )
            : base( OpCode.Message ) {
            MessageByte = messageByte;
            Text = text;
        }

        public MessagePacket( MessagePacket other )
            : base( OpCode.Message ) {
            MessageByte = other.MessageByte;
            Text = other.Text;
        }


        public override object Clone() {
            return new MessagePacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            MessageByte = data[1];
            Text = GetString( data, 2 );
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = MessageByte;
            SetString( data, 2, Text );
        }

        public override string ToString() {
            return String.Format( "{0}({1},\"{2}\")", OpCode, MessageByte, Text );
        }
    }
}