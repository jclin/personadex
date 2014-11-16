using GalaSoft.MvvmLight;
using Personadex.Collection;
using Personadex.Model;
using Personadex.Navigation;

namespace Personadex.ViewModel
{
    internal sealed class MainViewModel : ViewModelBase
    {
        private readonly IPersonaService _personaService;
        private readonly IPageNavigator _pageNavigator;

        private readonly ObservableVector<PersonaViewModel> _personaViewModels;
        public ObservableVector<PersonaViewModel> PersonaViewModels
        {
            get
            {
                return _personaViewModels;
            }
        }

        public MainViewModel(IPersonaService personaService, IPageNavigator pageNavigator)
        {
            _personaService     = personaService;
            _pageNavigator      = pageNavigator;
            _personaViewModels  = new ObservableVector<PersonaViewModel>(new PersonaViewModelProvider(personaService), IsInDesignMode);
        }
    }
}