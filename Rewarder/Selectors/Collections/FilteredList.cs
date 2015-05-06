using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Rewarder.Collections {
    public class FilteredList<T>: INotifyCollectionChanged, IEnumerable<T> {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private ObservableCollection<T> _source;

        private HashSet<IElementSelector<T>> _AllTrueSelectors = new HashSet<IElementSelector<T>>();

        public void AddOrSelector(IElementSelector<T> sel) {
            _AllTrueSelectors.Add(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Updated() {
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public FilteredList(ObservableCollection<T> sourceCollection) {
            _source = sourceCollection;
            _source.CollectionChanged += (sender, e) => {
                if (CollectionChanged != null) {
                    CollectionChanged(this, e);
                }
            };            

        }

        public IEnumerator<T> GetEnumerator() {
            return new FilteredListEnumerator(_source.GetEnumerator(), _AllTrueSelectors);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return new FilteredListEnumerator(_source.GetEnumerator(), _AllTrueSelectors);
        }

        private class FilteredListEnumerator: IEnumerator<T> {
            private IEnumerator<T> _base;
            private T _current;
            private IEnumerable<IElementSelector<T>> _OrSelectors;

            public FilteredListEnumerator(IEnumerator<T> baseEnumerator, IEnumerable<IElementSelector<T>> orSelectors) {
                _base = baseEnumerator;
                _OrSelectors = orSelectors;
            }

            public T Current {
                get { return _current; }
            }

            public void Dispose() {
                _base.Dispose();
            }

            object System.Collections.IEnumerator.Current {
                get { return _current; }
            }

            public bool MoveNext() {
                while (_base.MoveNext()) {
                    foreach (var sel in _OrSelectors) {
                        if (sel.isOk(_base.Current)) {
                            _current = _base.Current;
                            return true;
                        }
                    }
                }
                return false;
            }

            public void Reset() {
                _base.Reset();
            }
        }
    }
}
