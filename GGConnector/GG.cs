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

        public GG() { }

        public void Connect() {
            if (_socket != null) {
                _socket.Close();
            }

            _socket = new WebSocket(_serverAddres);

            _socket.OnMessage += CommonGGHandlers.MessageHandler;
            _socket.OnError += CommonGGHandlers.ErrorHandler;
            _socket.OnClose += CommonGGHandlers.CloseHandler;

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
    }
}
