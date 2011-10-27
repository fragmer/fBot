using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace fBot {
    sealed class ServerSession {
        const int Timeout = 10000;

        Thread thread;
        NetworkStream stream;
        PacketReader reader;
        PacketWriter writer;
        TcpClient client;
        public ServerInfo Info { get; private set; }

        readonly Queue<Packet> outputQueue = new Queue<Packet>();
        readonly object outputQueueLock = new object();

        public ServerSession( ServerInfo info ) {
            Info = info;
        }

        public void Connect( bool runInNewThread ) {
            client = new TcpClient {
                ReceiveTimeout = Timeout,
                SendTimeout = Timeout
            };
            client.Connect( Info.IP, Info.Port );
            stream = client.GetStream();
            reader = new PacketReader( stream );
            writer = new PacketWriter( stream );
            if( runInNewThread ) {
                thread = new Thread( IoThread ) {
                    IsBackground = true
                };
                thread.Start();
            } else {
                thread = Thread.CurrentThread;
                IoThread();
            }
        }

        void IoThread() {
            writer.Write( new HandshakePacket( Info.User, Info.AuthToken ) );
            while( true ) {
                while( stream.DataAvailable ) {
                    Packet packet = reader.ReadPacket();
                    if( PacketReceived != null ) {
                        PacketReceived( this, new PacketReceivedEventArgs( packet ) );
                    }
                    MessagePacket mp = packet as MessagePacket;
                    var h = MessageReceived;
                    if( mp != null && h != null ) {
                        h( this, new MessageReceivedEventArgs( mp.Text ) );
                    }
                }
                if( outputQueue.Count > 0 ) {
                    lock( outputQueueLock ) {
                        writer.Write( outputQueue.Dequeue() );
                    }
                }
            }
        }


        public void Send( Packet packet ) {
            lock( outputQueueLock ) {
                outputQueue.Enqueue( packet );
            }
        }
        public void Message( string text ) {
            writer.Write( new MessagePacket( text ) );
        }

        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }


    sealed class PacketReceivedEventArgs : EventArgs {
        public PacketReceivedEventArgs( Packet packet ) {
            Packet = packet;
        }
        public Packet Packet { get; private set; }
    }


    sealed class MessageReceivedEventArgs : EventArgs {
        public MessageReceivedEventArgs( string text ) {
            Text = text;
        }
        public string Text { get; private set; }
    }
}
