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
using System.Net;
using System.Text.RegularExpressions;

namespace TestApp {
    class Program {
        static void Main(string[] args) {
            //using (var gg = new GG()) {
            //    gg.Connect();
            //    gg.GetChannelsList(0, 2);

            //    gg.GetUsersList(6147);

            //    Console.ReadKey(true);
            //    Console.WriteLine("CLOSE CONNECTION");
            //}

            var req = (HttpWebRequest)WebRequest.Create("http://goodgame.ru/api/getchannelstatus?id=SoVa&fmt=json");
            var res = req.GetResponse();

            using (var stream = new StreamReader(res.GetResponseStream())) {
                var json = stream.ReadToEnd();
                Console.WriteLine("JSON: {0}", json);

                var regex = new Regex("^{\"[0-9]+\":");
                var match = regex.Match(json);
                var source = match.Value.Replace("{", "").Replace("\"", "").Replace(":","");

                Console.WriteLine("RESULT ID: {0}", int.Parse(source));

            }

            Console.ReadKey(true);
        }
    }
}
