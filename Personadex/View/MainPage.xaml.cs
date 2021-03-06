﻿using System;
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

            var personaViewModel  = args.Item as PersonaViewModel;
            if (personaViewModel != null)
            {
                var rootGrid = (Grid)(args.ItemContainer.ContentTemplateRoot);

                var nameTextBlock = (TextBlock)rootGrid.FindName("NameTextBlock");
                nameTextBlock.Text = personaViewModel.Name;
                nameTextBlock.Opacity = 1;
            }

            args.RegisterUpdateCallback(OnListViewShowArcanaTextBlockPhase);
        }

        private void OnListViewShowArcanaTextBlockPhase(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.Handled = true;

            if (args.Phase != 2)
            {
                throw new InvalidOperationException("This is not phase 2");
            }

            var personaViewModel = args.Item as PersonaViewModel;
            if (personaViewModel == null)
            {
                return;
            }

            var rootGrid = (Grid)(args.ItemContainer.ContentTemplateRoot);

            var arcanaTextBlock = (TextBlock)rootGrid.FindName("ArcanaTextBlock");
            arcanaTextBlock.Text = ((PersonaViewModel)args.Item).Arcana;
            arcanaTextBlock.Opacity = 1;
        }
    }
}
