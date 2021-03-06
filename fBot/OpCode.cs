﻿namespace fBot {
    public enum OpCode {
        Handshake = 0,
        Ping = 1,
        MapBegin = 2,
        MapChunk = 3,
        MapEnd = 4,
        SetBlockClient = 5,
        SetBlockServer = 6,
        AddEntity = 7,
        Teleport = 8,
        MoveRotate = 9,
        Move = 10,
        Rotate = 11,
        RemoveEntity = 12,
        Message = 13,
        Kick = 14,
        SetPermission = 15
    }


    public static class OpCodeExt {
        static readonly int[] PacketSizes = {
            131,    // Handshake
            1,      // Ping
            1,      // MapBegin
            1028,   // MapChunk
            7,      // MapEnd
            9,      // SetBlockClient
            8,      // SetBlockServer
            74,     // AddEntity
            10,     // Teleport
            7,      // MoveRotate
            5,      // Move
            4,      // Rotate
            2,      // RemoveEntity
            66,     // Message
            65,     // Kick
            2       // SetPermission
        };

        public static int GetPacketSize( this OpCode opCode ) {
            return PacketSizes[(int)opCode];
        }

        public static bool IsValidClientCode( this OpCode opCode ) {
            switch( opCode ) {
                case OpCode.Handshake:
                case OpCode.Ping:
                case OpCode.SetBlockClient:
                case OpCode.MoveRotate:
                case OpCode.Message:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsValidServerOpCode( this OpCode opCode ) {
            return (opCode >= OpCode.Handshake &&
                    opCode <= OpCode.SetPermission &&
                    opCode != OpCode.SetBlockClient);
        }
    }
}
