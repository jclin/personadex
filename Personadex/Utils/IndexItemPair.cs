namespace Personadex.Utils
{
    internal struct IndexItemPair<T> where T : class
    {
        private readonly int _index;
        public int Index
        {
            get
            {
                return _index;
            }
        }

        private readonly T _item;
        public T Item
        {
            get
            {
                return _item;
            }
        }

        public IndexItemPair(int index, T item)
        {
            _index = index;
            _item  = item;
        }
    }
}
