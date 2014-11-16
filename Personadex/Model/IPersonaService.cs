using System.Collections.Generic;

namespace Personadex.Model
{
    internal interface IPersonaService
    {
        long GetPersonaCount();
        IReadOnlyList<Persona> GetPersonas();
        Persona GetPersona(int personaId);
    }
}