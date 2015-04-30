using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

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
    }
}
