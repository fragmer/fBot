﻿using System;
using System.IO;
using System.Net;
using System.Text;
using JetBrains.Annotations;

namespace fBot {
    sealed class PacketWriter : BinaryWriter {
        public const byte ProtocolVersion = 7;

        public PacketWriter( Stream stream ) :
            base( stream ) { }

        public void Write( OpCode value ) {
            Write( (byte)value );
        }

        public void Write( Packet packet ) {
            Write( packet.Bytes );
        }

        public void WriteBE( short value ) {
            Write( IPAddress.HostToNetworkOrder( value ) );
        }

        public void WriteBE( int value ) {
            Write( IPAddress.HostToNetworkOrder( value ) );
        }

        public void WriteMCString( [NotNull] string value ) {
            if( value == null ) throw new ArgumentNullException( "value" );
            Write( Encoding.ASCII.GetBytes( value.PadRight( 64 ).Substring( 0, 64 ) ) );
        }
    }
}
