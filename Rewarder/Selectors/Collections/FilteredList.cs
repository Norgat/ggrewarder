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

        private HashSet<IElementSelector<T>> _OrSelectors = new HashSet<IElementSelector<T>>();
        private HashSet<IElementSelector<T>> _AndSelectors = new HashSet<IElementSelector<T>>();

        #region Add and Remove selectors
        public void AddOrSelector(IElementSelector<T> sel) {
            _OrSelectors.Add(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void AddAndSelector(IElementSelector<T> sel) {
            _AndSelectors.Add(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public bool isOrContained(IElementSelector<T> sel) {
            return _OrSelectors.Contains(sel);
        }

        public bool isAndContaied(IElementSelector<T> sel) {
            return _AndSelectors.Contains(sel);
        }

        public void RemoveOrSelector(IElementSelector<T> sel) {
            _OrSelectors.Remove(sel);
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void RemoveAndSelector(IElementSelector<T> sel) {
            _AndSelectors.Remove(sel);
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
            return new FilteredListEnumerator(_source.GetEnumerator(), _OrSelectors, _AndSelectors);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return new FilteredListEnumerator(_source.GetEnumerator(), _OrSelectors, _AndSelectors);
        }

        public static bool inResult(IEnumerable<IElementSelector<T>> _OrSelectors, IEnumerable<IElementSelector<T>> _AndSelectors, T elem) {
            foreach (var sel_or in _OrSelectors) {
                if (sel_or.isOk(elem)) {

                    var and_flag = true;
                    foreach (var sel_and in _AndSelectors) {
                        and_flag &= sel_and.isOk(elem);
                        if (!and_flag) {
                            break;
                        }
                    }

                    if (and_flag == true) {
                        return true;
                    }
                }
            }

            return false;
        }

        private class FilteredListEnumerator: IEnumerator<T> {
            private IEnumerator<T> _base;
            private IEnumerable<IElementSelector<T>> _OrSelectors;
            private IEnumerable<IElementSelector<T>> _AndSelectors;

            public FilteredListEnumerator(IEnumerator<T> baseEnumerator, IEnumerable<IElementSelector<T>> orSelectors, IEnumerable<IElementSelector<T>> AndSelectors) {
                _base = baseEnumerator;
                _OrSelectors = orSelectors;
                _AndSelectors = AndSelectors;
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
                    if (inResult(_OrSelectors, _AndSelectors, _base.Current)) {
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
