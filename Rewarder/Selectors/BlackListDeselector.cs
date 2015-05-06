using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class BlackListDeselector: IElementSelector<User> {

        private ICollection<User> _black;

        public BlackListDeselector(ICollection<User> blackList) {
            _black = blackList;
        }

        public bool isOk(User element) {
            if (_black == null) {
                return true;
            }
            return !_black.Any(U => U.id == element.id);
        }

        public int CompareTo(IElementSelector<User> other) {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override int GetHashCode() {
            if (_black != null) {
                return string.Format("BlackListDeselector<User> {0}", _black.GetHashCode()).GetHashCode();
            } else {
                return string.Format("BlackListDeselector<User>").GetHashCode();
            }
        }
    }
}
