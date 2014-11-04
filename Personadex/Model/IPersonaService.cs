using System.Collections.Generic;

namespace Personadex.Model
{
    internal interface IPersonaService
    {
        IReadOnlyList<Persona> GetPersonas();
    }
}