using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Personadex.Utils
{
    internal static class UIElementExtensions
    {
        public static T FindChild<T>(this UIElement rootElement) where T : UIElement
        {
            int childCount = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(rootElement, i) as UIElement;
                if (child == null)
                {
                    continue;
                }

                if (child is T)
                {
                    return child as T;
                }

                child = FindChild<T>(child);
                if (child != null)
                {
                    return child as T;
                }
            }

            return null;
        }

    }
}
