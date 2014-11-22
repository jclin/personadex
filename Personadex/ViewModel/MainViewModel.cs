using GalaSoft.MvvmLight;
using Personadex.Collection;
using Personadex.Model;
using Personadex.Navigation;
using Personadex.View;

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

        private PersonaViewModel _selectedPersonaViewModel;
        public PersonaViewModel SelectedPersonaViewModel
        {
            get
            {
                if (IsInDesignMode)
                {
                    return (PersonaViewModel)_personaViewModels[1];
                }

                return _selectedPersonaViewModel;
            }

            set
            {
                if (Set("SelectedPersonaViewModel", ref _selectedPersonaViewModel, value) && value != null)
                {
                    _pageNavigator.Navigate<PersonaDetailsPage>();
                }
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