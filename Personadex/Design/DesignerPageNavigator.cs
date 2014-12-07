using Windows.UI.Xaml.Controls;
using Personadex.Navigation;

namespace Personadex.Design
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
