using System;
using Windows.Foundation.Collections;
using GalaSoft.MvvmLight;
using Personadex.Navigation;
using Personadex.View;

namespace Personadex.ViewModel
{
    internal sealed class MainViewModel : ViewModelBase
    {
        private readonly IPageNavigator _pageNavigator;

        private readonly IObservableVector<object> _personaViewModels;
        public IObservableVector<object> PersonaViewModels
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
                if (!Set(ref _selectedPersonaViewModel, value))
                {
                    return;
                }

                if (_selectedPersonaViewModel == null)
                {
                    return;
                }

                _pageNavigator.Navigate<PersonaDetailsPage>(true);
            }
        }

        public MainViewModel(IObservableVector<object> personaViewModels, IPageNavigator pageNavigator)
        {
            _personaViewModels         = personaViewModels;
            _pageNavigator             = pageNavigator;
            _pageNavigator.NavigateTo += OnNavigateTo;
        }

        private void OnNavigateTo(object sender, Type pageNavigateToType)
        {
            if (pageNavigateToType != typeof (MainPage))
            {
                return;
            }

            // User navigated back to the main page, no persona should be selected
            SelectedPersonaViewModel = null;
        }
    }
}