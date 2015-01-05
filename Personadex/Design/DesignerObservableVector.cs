using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;
using Newtonsoft.Json.Linq;
using Personadex.Collection;
using Personadex.Model;
using Personadex.Suspension;
using Personadex.Utils;
using Personadex.ViewModel;

namespace Personadex.Design
{
    internal sealed class DesignerObservableVector : IVirtualizingVector<PersonaViewModel>
    {
        private readonly List<PersonaViewModel> _internalList;
        private readonly IPersonaService _personaService;

        public int Count
        {
            get
            {
                return (int)_personaService.GetPersonaCount();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public object this[int index]
        {
            get
            {
                return _internalList[index];
            }

            set
            {
            }
        }

        public event VectorChangedEventHandler<object> VectorChanged;

        public DesignerObservableVector(IPersonaService personaService)
        {
            _internalList   = new List<PersonaViewModel>();
            _personaService = personaService;

            var personas    = _personaService.GetPersonas(new Range(0U, (uint) _personaService.GetPersonaCount() - 1));
            _internalList.AddRange(personas.Select(persona => new PersonaViewModel(persona)));
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
        }

        public void Clear()
        {
        }

        public bool Contains(object item)
        {
            return _internalList.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
        }

        public bool Remove(object item)
        {
            return false;
        }

        public int IndexOf(object item)
        {
            return _internalList.IndexOf(item as PersonaViewModel);
        }

        public void Insert(int index, object item)
        {
        }

        public void RemoveAt(int index)
        {
        }

        string IJsonSerializable.Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        JToken IJsonSerializable.Write()
        {
            return new JObject();
        }

        void IJsonSerializable.Read(JToken value)
        {
        }
    }
}
