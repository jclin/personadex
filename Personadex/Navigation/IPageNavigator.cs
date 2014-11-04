using Windows.UI.Xaml.Controls;

namespace Personadex.Navigation
{
    public interface IPageNavigator
    {
        bool Navigate<TPage>() where TPage : Page;

        bool Navigate<TPage>(object paramter) where TPage : Page;
        
        void GoBack();
    }
}
