using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class WhiteListSelector: IElementSelector<User> {

        protected ICollection<User> _white = null;


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
            return string.Format("WhiteListSelector<User> {0}", _white.GetHashCode()).GetHashCode();
        }
    }
}
