using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class PremiumSelector: IElementSelector<User> {
        public bool isOk(User element) {
            return element.premium;
        }
    }
}
