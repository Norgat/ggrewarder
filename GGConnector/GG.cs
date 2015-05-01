using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

using System.Runtime.Serialization;
using GGConnector.GGObjects.GGRequest;
using System.IO;
using System.Runtime.Serialization.Json;

namespace GGConnector {
    public class GG: IDisposable {
        private static String _serverAddres = "ws://chat.goodgame.ru:8081/chat/websocket";

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

        public void GetChannelsList(int start, int count) {
            // Не использовать _socket.IsAlive, т.к. сервер goodgame подвисает и не отвечает на последующий запрос.
            if (_socket != null) {
                var reqData = new ChannelsListData { start = start, cout = count };
                var req = new ChannelsListRequest { type = "get_channels_list", data = reqData };

                using (var ms = new MemoryStream()) {
                    var ser = new DataContractJsonSerializer(typeof(ChannelsListRequest));
                    ser.WriteObject(ms, req);                   

                    ms.Position = 0;
                    var reader = new StreamReader(ms);
                    var message = reader.ReadToEnd();                    
                    _socket.SendAsync(message);

                    Console.WriteLine("SEND: {0}", message);
                }
            }
        }
    }
}
