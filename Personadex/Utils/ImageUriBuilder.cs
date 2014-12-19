using System;

namespace Personadex.Utils
{
    internal static class ImageUriBuilder
    {
        public static Uri BuildUri(string personaName, ImageSize desiredSize)
        {
            return new Uri(
                string.Format(
                    "ms-appx:///Images/{0}_{1}.png",
                    personaName.ToLowerInvariant().Replace(" ", string.Empty),
                    desiredSize
                    )
                );
        }
    }
}
