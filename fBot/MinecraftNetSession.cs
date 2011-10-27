using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;


namespace fBot {
    sealed class MinecraftNetSession {
        const string RefererUri = "http://www.minecraft.net/",
                     LoginUri = "http://www.minecraft.net/login",
                     LoginSecureUri = "https://www.minecraft.net/login",
                     PlayUri = "http://www.minecraft.net/classic/play/";

        static readonly Regex PlayIP = new Regex( @"name=""server"" value=""([^""]+)""" ),
                              PlayPort = new Regex( @"name=""port"" value=""(\d+)""" ),
                              PlayAuthToken = new Regex( @"name=""mppass"" value=""([0-9a-f]+)""" );


        public MinecraftNetSession( string username, string password ) {
            Username = username;
            Password = password;
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsLoggedIn { get; private set; }


        public bool Login() {
            if( IsLoggedIn ) return true;
            DownloadString( LoginUri, RefererUri );
            string loginString = String.Format( "username={0}&password={1}",
                                                Uri.EscapeDataString( Username ),
                                                Uri.EscapeDataString( Password ) );
            string loginResponse = UploadString( LoginSecureUri, LoginUri, loginString );
            IsLoggedIn = (loginResponse.IndexOf( "Logged in as " + Username ) >= 0);
            return IsLoggedIn;
        }


        public ServerInfo GetServerInfo( string serverHash ) {
            if( !IsLoggedIn ) return null;
            string playPage = DownloadString( PlayUri + serverHash, RefererUri );
            string rawIP = PlayIP.Match( playPage ).Groups[1].Value;
            string rawPort = PlayPort.Match( playPage ).Groups[1].Value;
            string authToken = PlayAuthToken.Match( playPage ).Groups[1].Value;
            return new ServerInfo( IPAddress.Parse( rawIP ), Int32.Parse( rawPort ), Username, authToken );
        }


        #region Networking

        const string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.22) Gecko/20110902 Firefox/3.6.22";
        const int Timeout = 15000;

        readonly CookieContainer cookieJar = new CookieContainer();


        HttpWebResponse MakeRequest( string uri, string referer, string dataToPost ) {
            var request = (HttpWebRequest)WebRequest.Create( uri );
            request.UserAgent = UserAgent;
            request.ReadWriteTimeout = Timeout;
            request.Timeout = Timeout;
            request.Referer = referer;
            request.KeepAlive = true;
            request.CookieContainer = cookieJar;
            if( dataToPost != null ) {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.UTF8.GetBytes( dataToPost );
                request.ContentLength = data.Length;
                using( Stream stream = request.GetRequestStream() ) {
                    stream.Write( data, 0, data.Length );
                }
            }
            return (HttpWebResponse)request.GetResponse();
        }


        string DownloadString( string uri, string referer ) {
            var response = MakeRequest( uri, referer, null );
            using( Stream stream = response.GetResponseStream() ) {
                if(stream==null)throw new IOException();
                using( StreamReader reader = new StreamReader( stream ) ) {
                    return reader.ReadToEnd();
                }
            }
        }


        string UploadString( string uri, string referer, string dataToPost ) {
            var response = MakeRequest( uri, referer, dataToPost );
            using( Stream stream = response.GetResponseStream() ) {
                if( stream == null ) throw new IOException();
                using( StreamReader reader = new StreamReader( stream ) ) {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}