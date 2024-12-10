using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics.Platform;
using SkiaSharp;

namespace QuickstartConsole
{
    
    public static class ImageResizer
    {
        public static byte[] ResizeImage(byte[] imageData, int width, int height)
        {
            using var inputStream = new MemoryStream(imageData);
            using var original = SKBitmap.Decode(inputStream);

            if (original == null)
                return null;

            var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
            if (resized == null)
                return null;

            using var image = SKImage.FromBitmap(resized);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.ToArray();
        }
    }

}
