﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GGConnector.GGObjects;

namespace Rewarder.Selectors {
    public class Donat5Selector: IElementSelector<User> {
        public bool isOk(User element) {
            return (element.payments >= 10000);
        }

        public int CompareTo(IElementSelector<User> other) {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override int GetHashCode() {
            return "DotanSelector 5".GetHashCode();
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}

