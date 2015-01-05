using Newtonsoft.Json.Linq;
using Personadex.Model;
using Personadex.Suspension;
using Personadex.ViewModel;

namespace Personadex.Collection
{
    internal sealed class PersonaViewModelVector : ObservableVector<PersonaViewModel>, IVirtualizingVector<PersonaViewModel>
    {
        public PersonaViewModelVector(IBatchedItemProvider<PersonaViewModel> vectorItemProvider)
            : base(vectorItemProvider)
        {
        }

        protected override string JsonName
        {
            get
            {
                return "PersonaViewModelVector";
            }
        }

        protected override JToken WriteItem(PersonaViewModel item)
        {
            return ((IJsonSerializable)item).Write();
        }

        protected override PersonaViewModel ReadItem(JToken jsonToken)
        {
            var persona = new Persona();
            ((IJsonSerializable)persona).Read(jsonToken);

            return new PersonaViewModel(persona);
        }
    }
}
