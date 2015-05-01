using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {

    [DataContract]
    class UsersListRequestData {
        [DataMember(Name = "channel_id")]
        public int channel_id { get; set; }
    }

    [DataContract]
    class UsersListRequest {
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "data")]
        public UsersListRequestData data { get; set; }
    }
}
