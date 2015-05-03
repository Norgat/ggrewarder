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
            using (var gg = new GG()) {
                gg.OnGetUsersList += (sender, users) => {
                    Console.WriteLine("USERS LIST RECIVED FOR CHANNEL {0}", users.channel_id);
                    Console.WriteLine("COUNT(users) = {0}", users.users_count);
                };

                gg.OnGetChannelsList += (sender, channels) => {
                    Console.WriteLine("CHANNELS LIST RECIVED");
                    Console.WriteLine("COUT(channels) = {0}", channels.channels.Count);

                    var channel = channels.channels.First();
                    Console.WriteLine("FIRST CHANNEL ID: {0}", channel.channel_id);
                };

                gg.OnMessageRecieved += (sender, message) => {
                    Console.WriteLine("MESSAGE {0}: {1}", message.user_name, message.text);
                };

                gg.Connect();
                //gg.GetChannelsList(0, 2);

                gg.GetUsersList(6147);
                gg.Join(6147);

                Console.ReadKey(true);

                gg.Unjoin(6147);
                Console.WriteLine("CLOSE CONNECTION");
            }            

            Console.WriteLine(GG.GetChannelId("BSP"));
            Console.ReadKey(true);
        }
    }
}
