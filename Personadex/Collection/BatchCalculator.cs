using System;
using Personadex.Utils;

namespace Personadex.Collection
{
    internal sealed class BatchCalculator
    {
        private readonly uint _totalItems;
        private readonly uint _itemsPerBatch;

        public BatchCalculator(uint totalItems, uint itemsPerBatch)
        {
            if (totalItems == 0)
            {
                throw new ArgumentException("totalItems must be greater than 0");
            }
            if (itemsPerBatch == 0)
            {
                throw new ArgumentException("itemsPerBatch must be greater than 0");
            }
            if (itemsPerBatch > totalItems)
            {
                throw new ArgumentException("itemsPerBatch cannot be greater than totalItems");
            }

            _totalItems    = totalItems;
            _itemsPerBatch = itemsPerBatch;
        }

        public Range CalculateIndexRange(uint itemIndex)
        {
            if (itemIndex + 1 > _totalItems)
            {
                throw new IndexOutOfRangeException("itemIndex cannot exceed the total number of items");
            }

            uint batchNumber = CalculateOneBasedBatchNumber(itemIndex + 1);
            uint start       = batchNumber * _itemsPerBatch - _itemsPerBatch + 1;
            uint end         = batchNumber * _itemsPerBatch;

            if (end > _totalItems)
            {
                end = _totalItems;
            }

            return new Range(start - 1, end - 1);
        }

        private uint CalculateOneBasedBatchNumber(uint itemOneBasedIndex)
        {
            return (itemOneBasedIndex / _itemsPerBatch) + (itemOneBasedIndex % _itemsPerBatch > 0 ? (uint)1 : 0);
        }
    }
}
