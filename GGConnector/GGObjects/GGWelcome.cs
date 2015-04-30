using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {

    [DataContract]
    class GGResponseWelcome {
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "data")]
        public GGWelcome welcome { get; set; }
    }

    [DataContract]
    class GGWelcome {
        [DataMember(Name = "protocolVersion")]
        public string protocol { get; set; }

        [DataMember(Name = "serverIdent")]
        public string serverIdent { get; set; }
    }
}
