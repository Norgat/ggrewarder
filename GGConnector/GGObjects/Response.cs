using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace GGConnector.GGObjects {   
    [DataContract]
    public class Response {
        [DataMember(Name = "type")]
        public string type { get; set; }
    }
}
