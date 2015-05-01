using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {
    [DataContract]
    class JoinRequestData {
        [DataMember(Name = "channel_id")]
        public int channel_id { get; set; }

        [DataMember(Name = "hidden")]
        public bool hidden { get; set; }
    }

    [DataContract]
    class JoinRequest {
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "data")]
        public JoinRequestData data { get; set; }
    }

    [DataContract]
    class Join {
        [DataMember(Name = "channel_id")]
        public int channel_id { get; set; }

        [DataMember(Name = "channel_name")]
        public string channel_name { get; set; }

        [DataMember(Name = "motd")]
        public string motd { get; set; }

        [DataMember(Name = "slowmod")]
        public int slowmod { get; set; }

        [DataMember(Name = "smiles")]
        public int smiles { get; set; }

        [DataMember(Name = "smilePeka")]
        public int smilePeka { get; set; }

        [DataMember(Name = "clients_in_channel")]
        public int clients_in_channel { get; set; }

        [DataMember(Name = "users_in_channel")]
        public int users_in_channel { get; set; }

        [DataMember(Name = "user_id")]
        public int user_id { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "access_rights")]
        public string access_rights { get; set; }

        [DataMember(Name = "premium")]
        public bool premium { get; set; }

        [DataMember(Name = "is_banned")]
        public bool is_banned { get; set; }

        [DataMember(Name = "banned_time")]
        public string banned_time { get; set; }

        [DataMember(Name = "reason")]
        public string reason { get; set; }

        [DataMember(Name = "payments")]
        public string payments { get; set; }

        [DataMember(Name = "paidsmiles")]
        public List<string> paidsmiles { get; set; }
    }

    [DataContract]
    class JoinResponse {
        [DataMember(Name = "data")]
        public Join data { get; set; }
    }
}
