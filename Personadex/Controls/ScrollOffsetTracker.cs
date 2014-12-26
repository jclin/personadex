using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Personadex.Utils;

namespace Personadex.Controls
{
    internal sealed class ScrollOffsetTracker : ContentControl
    { 
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(
                "VerticalOffset",
                typeof (double),
                typeof (ScrollOffsetTracker),
                new PropertyMetadata(0D, OnVerticalOffsetPropertyChanged)
                );

        private static readonly DependencyProperty ScrollViewerVerticalOffsetProperty =
            DependencyProperty.Register(
                "ScrollViewerVerticalOffset",
                typeof(double),
                typeof(ScrollOffsetTracker),
                new PropertyMetadata(0D, OnScrollViewerVerticalOffsetPropertyChanged)
                );

        private WeakReference<ScrollViewer> _scrollViewerReference;

        public double VerticalOffset
        {
            get
            {
                return (double)GetValue(VerticalOffsetProperty);
            }

            set
            {
                SetValue(VerticalOffsetProperty, value);
            }
        }

        public ScrollOffsetTracker()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            var content = Content as FrameworkElement;
            if (content == null)
            {
                return;
            }

            var scrollViewer = content.FindChild<ScrollViewer>();
            if (scrollViewer == null)
            {
                return;
            }

            if (VerticalOffset > double.Epsilon)
            {
                scrollViewer.ScrollToVerticalOffset(VerticalOffset);
            }

            _scrollViewerReference = new WeakReference<ScrollViewer>(scrollViewer);

            BindingOperations.SetBinding(
                this,
                ScrollViewerVerticalOffsetProperty,
                new Binding
                {
                    Source = scrollViewer,
                    Path   = new PropertyPath("VerticalOffset"),
                    Mode   = BindingMode.OneWay
                }
            );
        }

        private void OnScrollViewerVerticalOffsetChanged(double oldValue, double newValue)
        {
            if (Math.Abs(newValue - VerticalOffset) < double.Epsilon)
            {
                return;
            }

            VerticalOffset = newValue;
        }

        private void OnVerticalOffsetChanged(double oldValue, double newValue)
        {
            ScrollViewer scrollViewer;
            if (_scrollViewerReference == null ||
                !_scrollViewerReference.TryGetTarget(out scrollViewer))
            {
                return;
            }

            if (Math.Abs(newValue - scrollViewer.VerticalOffset) < double.Epsilon)
            {
                return;
            }

            scrollViewer.ScrollToVerticalOffset(newValue);
        }

        private static void OnScrollViewerVerticalOffsetPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var offsetTracker = obj as ScrollOffsetTracker;
            if (offsetTracker == null)
            {
                return;
            }

            offsetTracker.OnScrollViewerVerticalOffsetChanged((double) e.OldValue, (double) e.NewValue);
        }

        private static void OnVerticalOffsetPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var offsetTracker = obj as ScrollOffsetTracker;
            if (offsetTracker == null)
            {
                return;
            }

            offsetTracker.OnVerticalOffsetChanged((double)e.OldValue, (double)e.NewValue);
        }
    }
}
