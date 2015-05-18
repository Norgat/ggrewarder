using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class InChatDeselector: IElementSelector<User> {
        private IEnumerable<User> _black;

        public InChatDeselector(IEnumerable<User> blackList) {
            _black = blackList;
        }

        public bool isOk(User element) {
            if (_black == null) {
                return false;
            }

            return !(_black.Any(U => U.id == element.id));
        }

        public int CompareTo(IElementSelector<User> other) {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override int GetHashCode() {
            if (_black != null) {
                return string.Format("InChatDeselector<User> {0}", _black.GetHashCode()).GetHashCode();
            } else {
                return string.Format("InChatDeselector<User>").GetHashCode();
            }
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
