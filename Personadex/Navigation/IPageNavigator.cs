using System;
using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    public delegate void NavigateToDelegate(object sender, Type navigateToPageType);

    public interface IPageNavigator
    {
        event NavigateToDelegate NavigateTo;

        bool Navigate<TPage>() where TPage : Page;

        bool Navigate<TPage>(object parameter) where TPage : Page;
        
        void GoBack();
    }
}
