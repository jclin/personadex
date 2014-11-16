using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly bool _forceSynchronousOperations;
        private readonly IVectorItemProvider<T> _vectorItemProvider;

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
                        CreateItemAsync(index);
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

                GetCountAsync();
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

        public ObservableVector(IVectorItemProvider<T> vectorItemProvider, bool forceSynchronousOperations = false)
        {
            _vectorItemProvider              = vectorItemProvider;
            _forceSynchronousOperations      = forceSynchronousOperations;
            _dictionaryLock                  = new object();
            _internalDictionary              = new Dictionary<int, T>();
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

        private void CreateItemAsync(int forIndex)
        {
            var createItemTask = new Task<T>(() => _vectorItemProvider.CreateItem(forIndex));
            createItemTask.ContinueWith(
                task =>
                {
                    lock (_dictionaryLock)
                    {
                        _internalDictionary[forIndex] = task.Result;
                    }

                    Debug.Assert(task.Result != null, "Oops, no item was actually created");

                    NotifyVectorChanged(new VectorChangedEventArgs(CollectionChange.ItemChanged, (uint)forIndex));
                    NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, task.Result, null, forIndex));
                }, 
                TaskScheduler.FromCurrentSynchronizationContext()
            );

            if (_forceSynchronousOperations)
            {
                createItemTask.RunSynchronously(TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                createItemTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void GetCountAsync()
        {
            var getCountTask = new Task<long>(() => _vectorItemProvider.GetCount());
            getCountTask.ContinueWith(
                task =>
                {
                    Count = (int)task.Result;
                    NotifyVectorChanged(new VectorChangedEventArgs(CollectionChange.Reset, 0));
                    NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                },
                TaskScheduler.FromCurrentSynchronizationContext()
            );

            if (_forceSynchronousOperations)
            {
                getCountTask.RunSynchronously(TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                getCountTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
            }
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

        private static void AssertIsT(object item)
        {
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
