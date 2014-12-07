using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Personadex.Collection;
using Personadex.Model;
using Personadex.Utils;

namespace Personadex.ViewModel
{
    internal sealed class PersonaViewModelProvider : IBatchedItemProvider<PersonaViewModel>
    {
        private readonly IPersonaService _personaService;
        private readonly long _cachedCount;

        private readonly BatchCalculator _batchCalculator;
        private readonly Dictionary<Range, BatchStatus> _retrievalStatuses;
        private readonly object _retrievalLock;

        public event EventHandler<long> CountRetrieved;
        public event EventHandler<BatchRetrievedEventArgs<PersonaViewModel>> ItemBatchRetrieved;

        public PersonaViewModelProvider(IPersonaService personaService, uint itemsPerBatch = 1)
        {
            _personaService             = personaService;
            _cachedCount                = _personaService.GetPersonaCount();

            _batchCalculator            = new BatchCalculator((uint)_cachedCount, itemsPerBatch);
            _retrievalStatuses          = new Dictionary<Range, BatchStatus>();
            _retrievalLock              = new object();
        }

        public void GetCountAsync()
        {
            var getCountTask = new Task<long>(() => _cachedCount);

            getCountTask.ContinueWith(
                task => OnCountRetrieved(task.Result),
                TaskScheduler.FromCurrentSynchronizationContext()
                );

            RunTask(getCountTask);
        }

        public void RetrieveBatchForItemAsync(int vectorIndex)
        {
            Range indexRange = _batchCalculator.CalculateIndexRange((uint)vectorIndex);

            if (!CompareExchangeBatchStatus(indexRange, BatchStatus.NotSet, BatchStatus.Pending))
            {
                Debug.WriteLine("RetrieveBatchForItemAsync: Batch {0} is already pending or retrieved.", indexRange);
                return;
            }

            var createItemTask = new Task<IReadOnlyList<Persona>>(() => _personaService.GetPersonas(indexRange));

            createItemTask.ContinueWith(
                task =>
                {
                    CompareExchangeBatchStatus(indexRange, BatchStatus.Pending, BatchStatus.Retrieved);
                    OnItemBatchRetrieved(task.Result);
                },
                TaskScheduler.FromCurrentSynchronizationContext()
            );

            RunTask(createItemTask);
        }

        private void OnCountRetrieved(long count)
        {
            var handler = CountRetrieved;
            if (handler != null)
            {
                handler(this, count);
            }
        }

        private void OnItemBatchRetrieved(IEnumerable<Persona> personas)
        {
            var handler = ItemBatchRetrieved;
            if (handler == null)
            {
                return;
            }

            var personaIndexItemPairs = personas
                .Select(persona => new IndexItemPair<PersonaViewModel>((int)persona.Id - 1, new PersonaViewModel(persona)))
                .ToList();

            handler(this, new BatchRetrievedEventArgs<PersonaViewModel>(personaIndexItemPairs));
        }

        private bool CompareExchangeBatchStatus(
            Range indexRange,
            BatchStatus statusToCheckFor,
            BatchStatus newStatusIfEqual)
        {
            lock (_retrievalLock)
            {
                if (!_retrievalStatuses.ContainsKey(indexRange))
                {
                    _retrievalStatuses[indexRange] = BatchStatus.NotSet;
                }

                if (_retrievalStatuses[indexRange] != statusToCheckFor)
                {
                    return false;
                }

                Debug.WriteLine("CompareExchangeRetrievalStatus: {0}: {1} --> {2}.", indexRange, statusToCheckFor, newStatusIfEqual);
                _retrievalStatuses[indexRange] = newStatusIfEqual;

                return true;
            }
        }

        private static void RunTask(Task task)
        {
            task.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }

        private enum BatchStatus
        {
            NotSet,
            Pending,
            Retrieved
        }
    }
}