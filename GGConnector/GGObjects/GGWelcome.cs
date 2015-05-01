using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {

    [DataContract]
    internal class GGResponseWelcome {
        [DataMember(Name = "data")]
        public GGWelcome welcome { get; set; }
    }

    [DataContract]
    public class GGWelcome {
        [DataMember(Name = "protocolVersion")]
        public string protocol { get; set; }

        [DataMember(Name = "serverIdent")]
        public string serverIdent { get; set; }
    }
}
