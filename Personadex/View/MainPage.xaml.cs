using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Personadex.ViewModel;

namespace Personadex.View
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void OnListViewContentContainerChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.Handled = true;

            if (args.Phase != 0)
            {
                throw new InvalidOperationException("This is not phase 0");
            }

            var rootGrid = (Grid)(args.ItemContainer.ContentTemplateRoot);

            var rectangleImagePlaceholder = (Rectangle) rootGrid.FindName("ImagePlaceholder");
            rectangleImagePlaceholder.Opacity = 1;

            var nameTextBlock = (TextBlock) rootGrid.FindName("NameTextBlock");
            nameTextBlock.Opacity = 0;
            nameTextBlock.Text = string.Empty;

            var arcanaTextBlock = (TextBlock) rootGrid.FindName("ArcanaTextBlock");
            arcanaTextBlock.Opacity = 0;
            arcanaTextBlock.Text = string.Empty;

            args.RegisterUpdateCallback(OnListViewShowNameTextBlockPhase);
        }

        private void OnListViewShowNameTextBlockPhase(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.Handled = true;

            if (args.Phase != 1)
            {
                throw new InvalidOperationException("This is not phase 1");
            }

            var rootGrid = (Grid)(args.ItemContainer.ContentTemplateRoot);

            var nameTextBlock = (TextBlock)rootGrid.FindName("NameTextBlock");
            nameTextBlock.Text = ((PersonaViewModel) args.Item).Name;
            nameTextBlock.Opacity = 1;

            args.RegisterUpdateCallback(OnListViewShowArcanaTextBlockPhase);
        }

        private void OnListViewShowArcanaTextBlockPhase(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.Handled = true;

            if (args.Phase != 2)
            {
                throw new InvalidOperationException("This is not phase 2");
            }

            var rootGrid = (Grid)(args.ItemContainer.ContentTemplateRoot);

            var arcanaTextBlock = (TextBlock)rootGrid.FindName("ArcanaTextBlock");
            arcanaTextBlock.Text = ((PersonaViewModel)args.Item).Arcana;
            arcanaTextBlock.Opacity = 1;
        }
    }
}
