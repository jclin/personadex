using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    internal sealed class DesignerPageNavigator : IPageNavigator
    {
        public event NavigateToDelegate NavigateTo;

        public bool Navigate<TPage>(bool goBackOnBackPress = false) where TPage : Page
        {
            return true;
        }

        public bool Navigate<TPage>(object parameter, bool goBackOnBackPress = false) where TPage : Page
        {
            return true;
        }

        public void GoBack()
        {
        }
    }
}
