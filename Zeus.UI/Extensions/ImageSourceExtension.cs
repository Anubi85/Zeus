using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Zeus.UI.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ImageSource"/> class.
    /// </summary>
    internal static class ImageSourceExtension
    {
        /// <summary>
        /// Convert a <see cref="ImageSource"/> object to <see cref="Icon"/>.
        /// </summary>
        /// <param name="imageSource">The <see cref="ImageSource"/> object that has to be converted.</param>
        /// <returns>The converted <see cref="Icon"/>.</returns>
        public static Icon ToIcon(this ImageSource imageSource)
        {
            if (imageSource == null)
            {
                return null;
            }

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(new Uri(imageSource.ToString())));
            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            Bitmap bmp = new Bitmap(stream);
            Icon ico = Icon.FromHandle(bmp.GetHicon());
            bmp.Dispose();
            return ico;
        }
    }
}
