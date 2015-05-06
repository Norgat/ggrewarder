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

        public void Updated() {
            if (CollectionChanged != null) {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        // Использовать для того, чтобы обновлять представление при обновлении других представлений
        public void Observe(INotifyCollectionChanged someCollection) {
            someCollection.CollectionChanged += (sender, e) => {
                if (CollectionChanged != null) {
                    CollectionChanged(this, e);
                }
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

        private class FilteredListEnumerator: IEnumerator<T> {
            private IEnumerator<T> _base;
            private T _current;
            private IEnumerable<IElementSelector<T>> _OrSelectors;
            private IEnumerable<IElementSelector<T>> _AndSelectors;

            public FilteredListEnumerator(IEnumerator<T> baseEnumerator, IEnumerable<IElementSelector<T>> orSelectors, IEnumerable<IElementSelector<T>> AndSelectors) {
                _base = baseEnumerator;
                _OrSelectors = orSelectors;
                _AndSelectors = AndSelectors;
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
                    foreach (var sel_or in _OrSelectors) {
                        if (sel_or.isOk(_base.Current)) {
                            _current = _base.Current;

                            var and_flag = true;
                            foreach (var sel_and in _AndSelectors) {
                                and_flag &= sel_and.isOk(_base.Current);
                                if (!and_flag) {
                                    break;
                                }
                            }

                            if (and_flag == true) {
                                return true;
                            }
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
