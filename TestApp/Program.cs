using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using GGConnector;

namespace TestApp {
    class Program {
        static void Main(string[] args) {
            using (var gg = new GG()) {
                gg.Connect();
                Console.ReadKey(true);
            }
        }
    }
}
