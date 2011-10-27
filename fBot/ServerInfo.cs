using System.Net;

namespace fBot {
    sealed class ServerInfo {
        public ServerInfo( IPAddress ip, int port, string user, string authToken ) {
            IP = ip;
            Port = port;
            User = user;
            AuthToken = authToken;
        }
        public IPAddress IP { get; private set; }
        public int Port { get; private set; }
        public string User { get; private set; }
        public string AuthToken { get; private set; }
    }
}