using System;
using Windows.Foundation.Collections;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using Personadex.Collection;
using Personadex.Navigation;
using Personadex.Suspension;
using Personadex.View;

namespace Personadex.ViewModel
{
    internal sealed class MainViewModel : ViewModelBase, IJsonSerializable
    {
        public const string JsonName = "MainViewModel";

        private readonly IPageNavigator _pageNavigator;

        private readonly IVirtualizingVector<PersonaViewModel> _personaViewModels;
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

        private double _verticalOffset;
        public double VerticalOffset
        {
            get
            {
                return _verticalOffset;
            }

            set
            {
                Set("VerticalOffset", ref _verticalOffset, value);
            }
        }

        public MainViewModel(IVirtualizingVector<PersonaViewModel> personaViewModels, IPageNavigator pageNavigator)
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

        string IJsonSerializable.Name
        {
            get
            {
                return JsonName;
            }
        }

        JToken IJsonSerializable.Write()
        {
            return new JObject(
                new JProperty("SelectedIndex", _personaViewModels.IndexOf(_selectedPersonaViewModel)),
                new JProperty("VerticalOffset", _verticalOffset),
                new JProperty(_personaViewModels.Name, _personaViewModels.Write())
                );
        }

        void IJsonSerializable.Read(JToken value)
        {
            _personaViewModels.Read(value[_personaViewModels.Name]);

            var selectedIndex = value["SelectedIndex"].ToObject<int>();
            if (selectedIndex >= 0)
            {
                _selectedPersonaViewModel = (PersonaViewModel)_personaViewModels[selectedIndex];
                RaisePropertyChanged(() => SelectedPersonaViewModel);
            }

            _verticalOffset = value["VerticalOffset"].ToObject<double>();
            RaisePropertyChanged(() => VerticalOffset);
        }
    }
}