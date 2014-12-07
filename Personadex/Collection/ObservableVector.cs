using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation.Collections;
using GalaSoft.MvvmLight;

namespace Personadex.Collection
{
    internal sealed class ObservableVector<T> : ObservableObject, IObservableVector<object>, INotifyCollectionChanged
        where T : class
    {
        public event VectorChangedEventHandler<object> VectorChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private const string NotMutableExceptionMessage =
            "This collection is meant for random access data virtualization and is not externally mutable";

        private readonly IBatchedItemProvider<T> _itemProvider;

        private readonly object _dictionaryLock;
        private readonly Dictionary<int, T> _internalDictionary;

        public object this[int index]
        {
            get
            {
                var item = default(T);
                lock (_dictionaryLock)
                {
                    if (!_internalDictionary.ContainsKey(index))
                    {
                        _itemProvider.RetrieveBatchForItemAsync(index);
                    }
                    else
                    {
                        item = _internalDictionary[index];
                    }
                }

                return item;
            }

            set
            {
                throw new InvalidOperationException(NotMutableExceptionMessage);
            }
        }

        private int? _count;
        public int Count
        {
            get
            {
                if (_count.HasValue)
                {
                    return _count.Value;
                }

                _itemProvider.GetCountAsync();

                return 0;
            }

            private set
            {
                if (Set("Count", ref _count, value))
                {
                    RaisePropertyChanged(() => Count);
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ObservableVector(IBatchedItemProvider<T> vectorItemProvider)
        {
            _itemProvider                    = vectorItemProvider;
            _dictionaryLock                  = new object();
            _internalDictionary              = new Dictionary<int, T>();

            _itemProvider.CountRetrieved     += OnItemCountRetrieved;
            _itemProvider.ItemBatchRetrieved += OnItemBatchRetrieved;
        }

        public IEnumerator<object> GetEnumerator()
        {
            lock (_dictionaryLock)
            {
                return _internalDictionary.Values.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            throw new InvalidOperationException(NotMutableExceptionMessage);
        }

        public void Clear()
        {
            throw new InvalidOperationException(NotMutableExceptionMessage);
        }

        public bool Contains(object item)
        {
            AssertIsT(item);

            lock (_dictionaryLock)
            {
                return _internalDictionary.Values.Contains((T)item);
            }
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            throw new InvalidOperationException(NotMutableExceptionMessage);
        }

        public bool Remove(object item)
        {
            throw new InvalidOperationException(NotMutableExceptionMessage);
        }

        public int IndexOf(object item)
        {
            AssertIsT(item);

            lock (_dictionaryLock)
            {
                foreach (var key in _internalDictionary.Keys)
                {
                    if (ReferenceEquals(_internalDictionary[key], item))
                    {
                        return key;
                    }
                }
            }

            return -1;
        }

        public void Insert(int index, object item)
        {
            throw new InvalidOperationException(NotMutableExceptionMessage);
        }

        public void RemoveAt(int index)
        {
            throw new InvalidOperationException(NotMutableExceptionMessage);
        }

        private void NotifyVectorChanged(VectorChangedEventArgs args)
        {
            var handler = VectorChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, args);
        }

        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs  args)
        {
            var handler = CollectionChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, args);
        }

        private void OnItemCountRetrieved(object sender, long count)
        {
            _itemProvider.CountRetrieved -= OnItemCountRetrieved;

            Count = (int)count;

            NotifyVectorChanged(new VectorChangedEventArgs(CollectionChange.Reset, 0));
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnItemBatchRetrieved(object sender, BatchRetrievedEventArgs<T> args)
        {
            lock (_dictionaryLock)
            {
                foreach (var indexItemPair in args.ItemBatch)
                {
                    _internalDictionary[indexItemPair.Index] = indexItemPair.Item;

                    NotifyVectorChanged(new VectorChangedEventArgs(CollectionChange.ItemChanged, (uint)indexItemPair.Index));
                }
            }
        }

        private static void AssertIsT(object item)
        {
            if (item == null)
            {
                return;
            }

            Debug.Assert(item is T, string.Format("Error: expected an instance of {0}", typeof(T)));
        }

        #region VectorChangedEventArgs class

        private struct VectorChangedEventArgs : IVectorChangedEventArgs
        {
            private readonly CollectionChange _collectionChange ;
            public CollectionChange CollectionChange
            {
                get
                {
                    return _collectionChange;
                }
            }

            private uint _index;
            public uint Index
            {
                get
                {
                    return _index;
                }
            }

            public VectorChangedEventArgs(CollectionChange collectionChange, uint index)
            {
                _collectionChange = collectionChange;
                _index = index;
            }
        }

        #endregion VectorChangedEventArgs class
    }
}
