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

        private HashSet<IElementSelector<T>> _Selectors = new HashSet<IElementSelector<T>>();
        private HashSet<IElementSelector<T>> _Deselectors = new HashSet<IElementSelector<T>>();

        #region Add and Remove selectors
        public void AddSelector(IElementSelector<T> sel) {
            _Selectors.Add(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void AddDeselector(IElementSelector<T> sel) {
            _Deselectors.Add(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public bool isOrContained(IElementSelector<T> sel) {
            return _Selectors.Contains(sel);
        }

        public bool isAndContaied(IElementSelector<T> sel) {
            return _Deselectors.Contains(sel);
        }

        public void RemoveOrSelector(IElementSelector<T> sel) {
            _Selectors.Remove(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void RemoveAndSelector(IElementSelector<T> sel) {
            _Deselectors.Remove(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        } 
        #endregion

        public void Updated() {
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        // Использовать для того, чтобы обновлять представление при обновлении других представлений
        public void Observe(INotifyCollectionChanged someCollection) {
            someCollection.CollectionChanged += (sender, e) => {
                Updated();
            };
        }

        public FilteredList(ObservableCollection<T> sourceCollection) {
            _source = sourceCollection;
            Observe(_source);
        }

        public IEnumerator<T> GetEnumerator() {
            return new FilteredListEnumerator(_source.GetEnumerator(), _Selectors, _Deselectors);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return new FilteredListEnumerator(_source.GetEnumerator(), _Selectors, _Deselectors);
        }

        public static bool inResult(IEnumerable<IElementSelector<T>> _Selectors, IEnumerable<IElementSelector<T>> _Deselectors, T elem) {
            foreach (var deselector in _Deselectors) {
                if (deselector.isOk(elem)) {
                    return false;
                }
            }

            foreach (var selector in _Selectors) {
                if (selector.isOk(elem)) {
                    return true;
                }
            }

            return false;
        }

        private class FilteredListEnumerator: IEnumerator<T> {
            private IEnumerator<T> _base;
            private IEnumerable<IElementSelector<T>> _selectors;
            private IEnumerable<IElementSelector<T>> _deselectors;

            public FilteredListEnumerator(IEnumerator<T> baseEnumerator, IEnumerable<IElementSelector<T>> selectors, IEnumerable<IElementSelector<T>> deselectors) {
                _base = baseEnumerator;
                _selectors = selectors;
                _deselectors = deselectors;
            }

            public T Current {
                get { return _base.Current; }
            }

            public void Dispose() {
                _base.Dispose();
            }

            object System.Collections.IEnumerator.Current {
                get { return _base.Current; }
            }

            public bool MoveNext() {
                while (_base.MoveNext()) {
                    if (inResult(_selectors, _deselectors, _base.Current)) {
                        return true;
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
