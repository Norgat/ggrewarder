using GGConnector.GGObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewarder.Selectors {
    class RightSelector: IElementSelector<User> {
        private int _level = 0;

        public RightSelector() {
            _level = 0;
        }

        public RightSelector(int level) {
            _level = level;
        }

        public bool isOk(User element) {
            return element.rights > _level;
        }

        public int CompareTo(IElementSelector<User> other) {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override int GetHashCode() {
            return string.Format("RightSelector {0}", _level).GetHashCode();
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
