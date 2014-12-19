using Windows.Foundation;

namespace Personadex.Utils
{
    internal sealed class ImageSize
    {
        public static readonly ImageSize Small = new ImageSize(new Size(41, 41));
        public static readonly ImageSize Large = new ImageSize(new Size(70, 70));

        private readonly Size _dimensions;

        private ImageSize(Size dimensions)
        {
            _dimensions = dimensions;
        }

        public override string ToString()
        {
            return string.Concat(_dimensions.Width, "x", _dimensions.Height);
        }

        public static implicit operator string(ImageSize imageSize)
        {
            return imageSize.ToString();
        }
    }
}