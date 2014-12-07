using Windows.Foundation.Collections;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Personadex.Collection;
using Personadex.Design;
using Personadex.Model;
using Personadex.Navigation;

namespace Personadex.ViewModel
{
    internal sealed class ViewModelLocator
    {
        private const int PersonasPerBatch = 50;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IPageNavigator, DesignerPageNavigator>();
                SimpleIoc.Default.Register<IPersonaService, DesignerPersonaService>();
                SimpleIoc.Default.Register<IObservableVector<object>, DesignerObservableVector>();
            }
            else
            {
                SimpleIoc.Default.Register<IPageNavigator, PageNavigator>();
                SimpleIoc.Default.Register<IBatchedItemProvider<PersonaViewModel>>(() => new PersonaViewModelProvider(new PersonaService(), PersonasPerBatch));
                SimpleIoc.Default.Register<IObservableVector<object>, ObservableVector<PersonaViewModel>>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}