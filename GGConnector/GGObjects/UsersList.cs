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

    [DataContract]
    class User {
        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "rights")]
        public int rights { get; set; }

        [DataMember(Name = "premium")]
        public bool premium { get; set; }

        [DataMember(Name = "payments")]
        public string payments { get; set; }

        [DataMember(Name = "mobile")]
        public string mobile { get; set; }

        [DataMember(Name = "hidden")]
        public bool hidden { get; set; }
    }

    [DataContract]
    class UsersList {
        [DataMember(Name = "channel_id")]
        public int channel_id { get; set; }

        [DataMember(Name = "clients_in_channel")]
        public int clients_count { get; set; }

        [DataMember(Name = "users_in_channel")]
        public int users_count { get; set; }

        [DataMember(Name = "users")]
        public List<User> users { get; set; }
    }

    [DataContract]
    class UsersListResponse {
        [DataMember(Name = "data")]
        public UsersList data { get; set; }
    }
}
