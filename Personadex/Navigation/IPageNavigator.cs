using System;
using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    public delegate void NavigateToDelegate(object sender, Type navigateToPageType);

    public interface IPageNavigator
    {
        event NavigateToDelegate NavigateTo;

        bool Navigate<TPage>(bool goBackOnBackPress = false) where TPage : Page;

        bool Navigate<TPage>(object parameter, bool goBackBackPress = false) where TPage : Page;
        
        void GoBack();
    }
}
