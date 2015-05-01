using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects.GGRequest {
    [DataContract]
    public class ChannelsListRequest {       

        [DataMember(Name = "data")]
        public ChannelsListData data { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }
    }

    [DataContract]
    public class ChannelsListData {

        [DataMember(Name = "start")]
        public int start { get; set; }

        [DataMember(Name = "count")]
        public int cout { get; set; }
    }
}
