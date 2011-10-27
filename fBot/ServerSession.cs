using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using JetBrains.Annotations;

namespace fBot {
    sealed class ServerSession {
        const int TimeoutDefault = 10000;

        int timeout;
        public int Timeout {
            get { return timeout; }
            set {
                timeout = value;
                var cl = client;
                if( cl != null ) {
                    cl.SendTimeout = timeout;
                    cl.ReceiveTimeout = timeout;
                }
            }
        }

        Thread thread;
        NetworkStream stream;
        PacketReader reader;
        PacketWriter writer;
        TcpClient client;
        public ServerInfo Info { get; private set; }

        readonly Queue<Packet> outputQueue = new Queue<Packet>();
        readonly object outputQueueLock = new object();

        public ServerSession( [NotNull] ServerInfo info ) {
            if( info == null ) throw new ArgumentNullException( "info" );
            Info = info;
            Timeout = TimeoutDefault;
        }


        public void Connect( bool runInNewThread ) {
            client = new TcpClient {
                ReceiveTimeout = timeout,
                SendTimeout = timeout
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
            try {
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
            } catch( Exception ex ) {
                var h = Disconnected;
                if( Disconnected != null ) h( this, new DisconnectedEventArgs( ex ) );
            }
        }


        public void Send( [NotNull] Packet packet ) {
            if( packet == null ) throw new ArgumentNullException( "packet" );
            lock( outputQueueLock ) {
                outputQueue.Enqueue( packet );
            }
        }


        public void Message( [NotNull] string text ) {
            if( text == null ) throw new ArgumentNullException( "text" );
            writer.Write( new MessagePacket( text ) );
        }


        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
    }


    sealed class DisconnectedEventArgs : EventArgs {
        public DisconnectedEventArgs( Exception ex ) {
            Exception = ex;
        }
        public Exception Exception { get; private set; }
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
