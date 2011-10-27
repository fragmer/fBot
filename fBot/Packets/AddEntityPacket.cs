using System;

namespace fBot {
    sealed class AddEntityPacket : Packet {
        public byte EntityId { get; set; }
        public string EntityName { get; set; }
        public Position EntityPosition { get; set; }


        public AddEntityPacket()
            : base( OpCode.AddEntity ) { }

        public AddEntityPacket( byte[] rawData )
            : base( OpCode.AddEntity, rawData ) {
        }

        public AddEntityPacket( byte entityId, string entityName, Position entityPosition )
            : base( OpCode.AddEntity ) {
            EntityId = entityId;
            EntityName = entityName;
            EntityPosition = entityPosition;
        }

        public AddEntityPacket( AddEntityPacket other )
            : base( OpCode.AddEntity ) {
            EntityId = other.EntityId;
            EntityName = other.EntityName;
            EntityPosition = other.EntityPosition;
        }


        public override object Clone() {
            return new AddEntityPacket( this );
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = EntityId;
            SetString( data, 2, EntityName );
            SetPosition( data, 66, EntityPosition );
        }

        protected override void FromBytes( byte[] data ) {
            EntityId = data[1];
            EntityName = GetString( data, 2 );
            EntityPosition = GetPosition( data, 66 );
        }

        public override string ToString() {
            return String.Format( "{0}({1},\"{2}\",{3})", OpCode, EntityId, EntityName, EntityPosition );
        }
    }
}