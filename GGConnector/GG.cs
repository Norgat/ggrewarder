using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

using GGConnector.GGObjects;
using System.Net;
using System.Text.RegularExpressions;

namespace GGConnector {
    public class GG: IDisposable {
        private static String _serverAddres = "ws://chat.goodgame.ru:8081/chat/websocket";

        // Не использовать _socket.IsAlive, т.к. сервер goodgame подвисает и не отвечает на последующий запрос.
        private WebSocket _socket = null;

        public delegate void UsersListRecived(object sender, UsersList users);
        public event UsersListRecived OnGetUsersList;

        public delegate void ChannelsListRecived(object sender, ChannelsList channels);
        public event ChannelsListRecived OnGetChannelsList;

        public delegate void WelcomeRecived(object sender, Welcome welcome);
        public event WelcomeRecived OnGetWelcome;

        public delegate void MessageRecieved(object sender, Message message);
        public event MessageRecieved OnMessageRecieved;

        public GG() { }

        public static T ParseJSONObject<T>(string data) {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(data))) {
                try {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(ms);
                } catch (Exception exp) {
                    throw exp;
                }
            }
        }

        public void MessageHandler(object sender, MessageEventArgs e) {
            try {
                //Console.WriteLine(e.Data);
                var resp = ParseJSONObject<Response>(e.Data);

                switch (resp.type) {
                    case "welcome":
                        var rWelcome = ParseJSONObject<ResponseWelcome>(e.Data);
                        if (OnGetWelcome != null) {
                            OnGetWelcome(this, rWelcome.welcome);
                        }
                        break;

                    case "channels_list":
                        var rChannelsList = ParseJSONObject<ChannelsListResponse>(e.Data);
                        if (OnGetChannelsList != null) {
                            OnGetChannelsList(this, rChannelsList.data);
                        }
                        break;

                    case "users_list":
                        var rUsersList = ParseJSONObject<UsersListResponse>(e.Data);
                        if (OnGetUsersList != null) {
                            OnGetUsersList(this, rUsersList.data);
                        }
                        break;

                    case "message":
                        var rMessage = ParseJSONObject<MessageResponse>(e.Data);
                        if (OnMessageRecieved != null) {
                            OnMessageRecieved(this, rMessage.data);
                        }
                        break;

                    default:
                        break;
                }
            } catch (Exception ex) {
                throw ex;
            }
        }

        public void ErrorHandler(object sender, WebSocketSharp.ErrorEventArgs e) {
            Console.WriteLine("ERROR: {0}", e.Message);
        }

        public void CloseHandler(object sender, CloseEventArgs e) {
            Console.WriteLine("WEBSOCKET CLOSED");
        }

        public void Connect() {
            if (_socket != null) {
                _socket.Close();
            }

            _socket = new WebSocket(_serverAddres);

            _socket.OnMessage += MessageHandler;
            _socket.OnError += ErrorHandler;
            _socket.OnClose += CloseHandler;            

            _socket.Connect();
        }

        public void Dispose() {
            if (_socket != null) {
                _socket.Close();
            }
        }

        public static string SerializeJSON<T>(T obj) {
            using (var ms = new MemoryStream()) {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                ms.Position = 0;
                return (new StreamReader(ms)).ReadToEnd();                
            }
        }

        public static int GetChannelId(string streamerName) {
            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("http://goodgame.ru/api/getchannelstatus?id={0}&fmt=json", streamerName));
            var res = req.GetResponse();

            using (var stream = new StreamReader(res.GetResponseStream())) {
                var json = stream.ReadToEnd();

                var regex = new Regex("^{\"[0-9]+\":");
                var match = regex.Match(json);
                var source = match.Value.Replace("{", "").Replace("\"", "").Replace(":", "");

                return int.Parse(source);
            }
        }

        public void GetChannelsList(int start, int count) {            
            if (_socket != null) {
                var reqData = new ChannelsListData { start = start, cout = count };
                var req = new ChannelsListRequest { type = "get_channels_list", data = reqData };
                _socket.SendAsync(SerializeJSON<ChannelsListRequest>(req));
            }
        }

        public void GetUsersList(int channel_id) {
            if (_socket != null) {
                var reqData = new UsersListRequestData { channel_id = channel_id };
                var req = new UsersListRequest { type = "get_users_list2", data = reqData };
                _socket.SendAsync(SerializeJSON<UsersListRequest>(req));
            }
        }

        public void Join(int channel_id) {
            if (_socket != null) {
                var reqData = new JoinRequestData { channel_id = channel_id, hidden = false };
                var req = new JoinRequest { type = "join", data = reqData };
                _socket.SendAsync(SerializeJSON<JoinRequest>(req));
            }
        }

        public void Unjoin(int channel_id) {
            if (_socket != null) {
                var reqData = new UnjoinRequestData { channel_id = channel_id };
                var req = new UnjoinRequest { type = "unjoin", data = reqData };
                _socket.SendAsync(SerializeJSON<UnjoinRequest>(req));
            }
        }
    }
}
