using System.Diagnostics;
using Personadex.Collection;
using Personadex.Model;

namespace Personadex.ViewModel
{
    internal sealed class PersonaViewModelProvider : IVectorItemProvider<PersonaViewModel>
    {
        private readonly IPersonaService _personaService;
        private long? _cachedCount;

        public PersonaViewModelProvider(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        public long GetCount()
        {
            return _cachedCount ?? (_cachedCount = _personaService.GetPersonaCount()).Value;
        }

        public PersonaViewModel CreateItem(int vectorIndex)
        {
            return new PersonaViewModel(_personaService.GetPersona(GetPersonaId(vectorIndex)));
        }

        private int GetPersonaId(int vectorIndex)
        {
            Debug.Assert(vectorIndex >= 0 && vectorIndex + 1 <= GetCount(), "vectorIndex is out of bounds");

            return vectorIndex + 1;
        }
    }
}