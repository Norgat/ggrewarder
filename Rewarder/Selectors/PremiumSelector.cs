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

        public int CompareTo(IElementSelector<User> other) {
            if (other != null) {
                return 0;
            } else {
                return 1;
            }
        }
    }
}
