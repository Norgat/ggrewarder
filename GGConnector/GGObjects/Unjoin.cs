using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {
    [DataContract]
    class UnjoinRequestData {
        [DataMember(Name = "channel_id")]
        public int channel_id { get; set; }
    }

    [DataContract]
    class UnjoinRequest {
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "data")]
        public UnjoinRequestData data { get; set; }
    }

    [DataContract]
    class UnjoinResponse {
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "data")]
        public UnjoinRequestData data { get; set; }
    }
}
