using System;

namespace fBot {
    sealed class SetPermissionPacket : Packet {
        public byte PermissionByte { get; set; }


        public SetPermissionPacket()
            : base( OpCode.SetPermission ) { }

        public SetPermissionPacket( byte[] rawData )
            : base( OpCode.SetPermission, rawData ) { }

        public SetPermissionPacket( byte permissionByte )
            : base( OpCode.SetPermission ) {
            PermissionByte = permissionByte;
        }

        public SetPermissionPacket( SetPermissionPacket other )
            : base( OpCode.SetPermission ) {
            PermissionByte = other.PermissionByte;
        }



        public override object Clone() {
            return new SetPermissionPacket( this );
        }

        protected override void FromBytes( byte[] data ) {
            PermissionByte = data[1];
        }

        protected override void ToBytes( byte[] data ) {
            data[1] = PermissionByte;
        }

        public override string ToString() {
            return String.Format( "{0}({1})", OpCode, PermissionByte );
        }
    }
}