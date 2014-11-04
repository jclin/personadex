using Windows.ApplicationModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Personadex.Model;
using Personadex.Navigation;

namespace Personadex.ViewModel
{
    internal sealed class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IPersonaService, DesignerPersonaService>();
                SimpleIoc.Default.Register<IPageNavigator, DesignerPageNavigator>();
            }
            else
            {
                SimpleIoc.Default.Register<IPersonaService, PersonaService>();
                SimpleIoc.Default.Register<IPageNavigator, PageNavigator>();
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