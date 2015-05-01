using GGConnector.GGObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace GGConnector {
    class CommonGGHandlers {
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

        public static void MessageHandler(object sernder, MessageEventArgs e) {
            Console.WriteLine("MESSAGE: {0}", e.Data);

            var resp = ParseJSONObject<GGResponse>(e.Data);

            if (resp.type == "welcome") {
                var rWelcome = ParseJSONObject<GGResponseWelcome>(e.Data);
                Console.WriteLine("PROTOCOL: {0}", rWelcome.welcome.protocol);
            }
            
        }

        public static void ErrorHandler(object sender, WebSocketSharp.ErrorEventArgs e) {
            Console.WriteLine("ERROR: {0}", e.Message);
        }

        public static void CloseHandler(object sender, CloseEventArgs e) {
            Console.WriteLine("WEBSOCKET CLOSED");
        }
    }
}
