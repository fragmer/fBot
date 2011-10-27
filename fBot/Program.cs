namespace fBot {
    static class Program {
        static void Main() {
            MinecraftNetSession login = new MinecraftNetSession( "fCraft", "HerpDerp" );
            login.Login();

            ServerInfo data = login.GetServerInfo( "f4e8bee64595a29f61cacd2fdae8479" );

            ServerSession session = new ServerSession( data );
            session.Connect( false );
        }
    }
}