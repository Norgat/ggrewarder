using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GGConnector.GGObjects {
    [DataContract]
    class Error {
        [DataMember(Name = "channel_id")]
        public int channel_id { get; set; }

        [DataMember(Name = "error_num")]
        public int error_num { get; set; }

        [DataMember(Name = "errorMsg")]
        public string errorMsg { get; set; }
    }
}
