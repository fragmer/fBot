﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;


namespace fBot {
    sealed class MinecraftNetSession {
        const string RefererUri = "http://www.minecraft.net/",
                     LoginUri = "http://www.minecraft.net/login",
                     LoginSecureUri = "https://www.minecraft.net/login",
                     PlayUri = "http://www.minecraft.net/classic/play/";

        static readonly Regex PlayIP = new Regex( @"name=""server"" value=""([^""]+)""" ),
                              PlayPort = new Regex( @"name=""port"" value=""(\d+)""" ),
                              PlayAuthToken = new Regex( @"name=""mppass"" value=""([0-9a-f]+)""" );

        static readonly Regex LoginAuthToken = new Regex( @"<input type=""hidden"" name=""authenticityToken"" value=""([0-9a-f]+)"">" );


        public string Username { get; private set; }
        public string Password { get; private set; }
        public LoginResult Status { get; private set; }

        public MinecraftNetSession( string username, string password ) {
            if( username == null ) throw new ArgumentNullException( "username" );
            if( password == null ) throw new ArgumentNullException( "password" );
            Username = username;
            Password = password;
        }


        public LoginResult Login() {
            string loginPage = DownloadString( LoginUri, RefererUri );

            string authToken = LoginAuthToken.Match( loginPage ).Groups[1].Value;

            string loginString = String.Format( "username={0}&password={1}&authenticityToken={2}",
                                                Uri.EscapeDataString( Username ),
                                                Uri.EscapeDataString( Password ),
                                                Uri.EscapeDataString( authToken ) );

            string loginResponse = UploadString( LoginSecureUri, LoginUri, loginString );
            if( loginResponse.Contains( "Oops, unknown username or password." ) ) {
                Status = LoginResult.WrongUsernameOrPass;
            } else if( loginResponse.IndexOf( "Logged in as " + Username ) != -1 ) {
                Status = LoginResult.Success;
            } else {
                Status = LoginResult.Error;
            }
            return Status;
        }


        public ServerInfo GetServerInfo( string serverHash ) {
            if( serverHash == null ) throw new ArgumentNullException( "serverHash" );
            if( Status != LoginResult.Success ) throw new InvalidOperationException( "Not logged in" );
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
                if( stream == null ) throw new IOException();
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