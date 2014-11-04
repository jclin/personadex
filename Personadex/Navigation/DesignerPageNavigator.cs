using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    internal sealed class DesignerPageNavigator : IPageNavigator
    {
        public bool Navigate<TPage>() where TPage : Page
        {
            return true;
        }

        public bool Navigate<TPage>(object paramter) where TPage : Page
        {
            return true;
        }

        public void GoBack()
        {
        }
    }
}
