using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using WebSocketSharp;
using GGConnector;
using GGConnector.GGObjects;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace TestApp {
    class Program {
        static void Main(string[] args) {
            using (var gg = new GG()) {
                gg.Connect();
                gg.GetChannelsList(0, 2);

                gg.GetUsersList(6147);

                Console.ReadKey(true);
                Console.WriteLine("CLOSE CONNECTION");
            }

            //var reqData = new ChannelsListData { start = 0, cout = 2 };
            //var req = new ChannelsListRequest { type = "get_channels_list", data = reqData };

            

            //using (var ws = new WebSocket("ws://chat.goodgame.ru:8081/chat/websocket")) {
            //    ws.OnMessage += (sender, e) => {
            //        Console.WriteLine("MSG: {0}", e.Data);
            //    };

            //    ws.OnError += (sender, e) => {
            //        Console.WriteLine("ERROR: {0}", e.Message);
            //    };

            //    ws.Connect();

            //    var m = "{'type':'get_channels_list','data':{'start':0,'count':2}}";

            //    using (var ms = new MemoryStream()) {
            //        var ser = new DataContractJsonSerializer(typeof(ChannelsListRequest));
            //        ser.WriteObject(ms, req);

            //        ms.Position = 0;
            //        var reader = new StreamReader(ms);
            //        var message = reader.ReadToEnd();
            //        Console.WriteLine("SEND: {0}", message);

            //        ws.Send(message);
            //    }

            //    Console.ReadKey(true);
            //}

            Console.ReadKey(true);
        }
    }
}
