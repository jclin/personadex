using System.Collections.Generic;
using Personadex.Utils;

namespace Personadex.Model
{
    internal interface IPersonaService
    {
        long GetPersonaCount();
        IReadOnlyList<Persona> GetPersonas(Range range);
    }
}