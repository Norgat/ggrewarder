using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewarder {
    public interface IElementSelector<T>: IComparable<IElementSelector<T>> {
        bool isOk(T element);

        // CompareTo реализовывать через GetHashCode
    }
}
