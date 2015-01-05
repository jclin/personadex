using Windows.UI.Xaml.Controls;
using Personadex.Navigation;

namespace Personadex.Design
{
    internal sealed class DesignerPageNavigator : IPageNavigator
    {
        public event NavigateToDelegate NavigateTo;

        public bool Navigate<TPage>() where TPage : Page
        {
            return true;
        }

        public bool Navigate<TPage>(object parameter) where TPage : Page
        {
            return true;
        }

        public void GoBack()
        {
        }
    }
}
