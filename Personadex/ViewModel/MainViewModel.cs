using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Personadex.Model;
using Personadex.Navigation;

namespace Personadex.ViewModel
{
    internal sealed class MainViewModel : ViewModelBase
    {
        private readonly IPersonaService _personaStore;
        private readonly IPageNavigator _pageNavigator;

        private readonly ObservableCollection<PersonaViewModel> _personaViewModels; 
        public ObservableCollection<PersonaViewModel> PersonaViewModels
        {
            get
            {
                return _personaViewModels;
            }
        }

        public MainViewModel(IPersonaService personaStore, IPageNavigator pageNavigator)
        {
            _personaStore       = personaStore;
            _pageNavigator      = pageNavigator;
            _personaViewModels  = new ObservableCollection<PersonaViewModel>();

            GeneratePersonaViewModels();
        }

        private async void GeneratePersonaViewModels()
        {
            IReadOnlyList<Persona> personas;

            var getPersonasTask = new Task<IReadOnlyList<Persona>>(() => _personaStore.GetPersonas());

            if (IsInDesignMode)
            {
                getPersonasTask.RunSynchronously();
                personas = getPersonasTask.Result;
            }
            else
            {
                getPersonasTask.Start();
                personas = await getPersonasTask;
            }

            foreach (var persona in personas)
            {
                _personaViewModels.Add(new PersonaViewModel(persona));
            }
        }
    }
}