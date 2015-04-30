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
        public static void MessageHandler(object sernder, MessageEventArgs e) {
            Console.WriteLine("MESSAGE: {0}", e.Data);

            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(e.Data))) {
                try {
                    var serializer = new DataContractJsonSerializer(typeof(GGResponse));
                    GGResponse resp = (GGResponse)serializer.ReadObject(ms);
                    Console.WriteLine("TYPE: {0}", resp.type);

                    if (resp.type == "welcome") {
                        ms.Position = 0;
                        var ser = new DataContractJsonSerializer(typeof(GGResponseWelcome));
                        GGResponseWelcome rWelcome = (GGResponseWelcome)ser.ReadObject(ms);

                        Console.WriteLine("PROTOCOL: {0}", rWelcome.welcome.protocol);
                    }
                } catch (Exception exp) {
                    throw exp;
                }
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
