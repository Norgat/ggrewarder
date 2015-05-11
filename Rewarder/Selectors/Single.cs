using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class Single: IElementSelector<User> {
        private int _id;

        public Single(int id) {
            _id = id;
        }

        public Single(User user) {
            _id = user.id;
        }

        public bool isOk(User element) {
            return _id == element.id;
        }

        public int CompareTo(IElementSelector<User> other) {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override int GetHashCode() {
            return string.Format("Single {0}", _id).GetHashCode();
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
