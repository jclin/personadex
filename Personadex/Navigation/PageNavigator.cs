using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    internal sealed class PageNavigator : IPageNavigator
    {
        public event NavigateToDelegate NavigateTo;

        private static Frame Frame
        {
            get
            {
                return (Frame) Window.Current.Content;
            }
        }

        public PageNavigator()
        {
            Frame.Navigated             += OnPageNavigated;
            HardwareButtons.BackPressed += OnHardwareButtonsBackPressed;
        }

        public bool Navigate<TPage>() where TPage : Page
        {
            return Navigate<TPage>(null);
        }

        public bool Navigate<TPage>(object parameter) where TPage : Page
        {
            return Frame.Navigate(typeof (TPage), parameter);
        }

        public void GoBack()
        {
            Frame.GoBack();
        }

        private void OnHardwareButtonsBackPressed(object sender, BackPressedEventArgs e)
        {
            if (!Frame.CanGoBack)
            {
                return;
            }

            GoBack();
            e.Handled = true;
        }

        private void OnPageNavigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var navigateToHandler = NavigateTo;
            if (navigateToHandler != null)
            {
                navigateToHandler(this, e.SourcePageType);
            }
        }
    }
}
