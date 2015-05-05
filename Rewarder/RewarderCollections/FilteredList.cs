using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Rewarder.RewarderCollections {
    public class FilteredList<T>: INotifyCollectionChanged, IEnumerable<T> {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private ObservableCollection<T> _source;

        public FilteredList(ObservableCollection<T> sourceCollection) {
            _source = sourceCollection;
            _source.CollectionChanged += (sender, e) => {
                this.CollectionChanged(this, e);
            };
        }

        public IEnumerator<T> GetEnumerator() {
            return _source.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _source.GetEnumerator();
        }
    }
}
