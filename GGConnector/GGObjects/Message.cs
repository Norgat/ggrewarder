using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {
    [DataContract]
    public class Message {
        [DataMember(Name = "user_id")]
        public int user_id { get; set; }

        [DataMember(Name = "user_name")]
        public string user_name { get; set; }

        [DataMember(Name = "user_group")]
        public int user_group { get; set; }

        [DataMember(Name = "message_id")]
        public int message_id { get; set; }

        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }

        [DataMember(Name = "text")]
        public string text { get; set; }
    }

    [DataContract]
    class MessageResponse {
        [DataMember(Name = "data")]
        public Message data { get; set; }
    }
}
