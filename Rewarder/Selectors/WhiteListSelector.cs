using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class WhiteListSelector: IElementSelector<User> {

        protected IEnumerable<User> _white;

        public WhiteListSelector(IEnumerable<User> whiteList) {
            _white = whiteList;
        }


        public bool isOk(User element) {
            if (_white == null) {
                return false;
            }

            return _white.Any(U => U.id == element.id);
        }

        public int CompareTo(IElementSelector<User> other) {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override int GetHashCode() {
            return string.Format("WhiteListSelector<User>").GetHashCode();
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
