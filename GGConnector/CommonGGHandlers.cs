using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace GGConnector {
    class CommonGGHandlers {
        public static void MessageHandler(object sernder, MessageEventArgs e) {
            Console.WriteLine("MESSAGE: {0}", e.Data);
        }

        public static void ErrorHandler(object sender, ErrorEventArgs e) {
            Console.WriteLine("ERROR: {0}", e.Message);
        }

        public static void CloseHandler(object sender, CloseEventArgs e) {
            Console.WriteLine("WEBSOCKET CLOSED");
        }
    }
}
