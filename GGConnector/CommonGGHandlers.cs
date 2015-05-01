using GGConnector.GGObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

using GGConnector.GGObjects.GGRequest;

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

            switch (resp.type) {
                case "welcome":
                    var rWelcome = ParseJSONObject<GGResponseWelcome>(e.Data);
                    Console.WriteLine("PROTOCOL: {0}", rWelcome.welcome.protocol);
                    break;

                case "channels_list":
                    var rChannelsList = ParseJSONObject<ChannelsListResponse>(e.Data);
                    Console.WriteLine("CHANNELS: {0}", rChannelsList.data.channels.Count);
                    break;

                default:
                    break;
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
