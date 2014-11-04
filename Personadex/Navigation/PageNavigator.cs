using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    internal sealed class PageNavigator : IPageNavigator
    {
        public bool Navigate<TPage>() where TPage : Page
        {
            return ((Frame) Window.Current.Content).Navigate(typeof (TPage));
        }

        public bool Navigate<TPage>(object paramter) where TPage : Page
        {
            return ((Frame) Window.Current.Content).Navigate(typeof (TPage), paramter);
        }

        public void GoBack()
        {
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}
