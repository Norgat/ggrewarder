using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {

    [DataContract]
    class ResponseWelcome {
        [DataMember(Name = "data")]
        public Welcome welcome { get; set; }
    }

    [DataContract]
    public class Welcome {
        [DataMember(Name = "protocolVersion")]
        public string protocol { get; set; }

        [DataMember(Name = "serverIdent")]
        public string serverIdent { get; set; }
    }
}
