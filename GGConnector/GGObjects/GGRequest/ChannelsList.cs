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

    [DataContract]
    public class Channel {
        [DataMember(Name = "channel_id")]
        public int id { get; set; }

        [DataMember(Name = "channel_name")]
        public string name { get; set; }

        [DataMember(Name = "clients_in_channel")]
        public int clients { get; set; }

        [DataMember(Name = "users_in_channel")]
        public int users { get; set; }
    }


    [DataContract]
    public class ChannelsList {
        [DataMember(Name = "channels")]
        public List<Channel> channels { get; set; }
    }


    [DataContract]
    public class ChannelsListResponse {
        [DataMember(Name = "data")]
        public ChannelsList data { get; set; }
    }
}
