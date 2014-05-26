using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace WotDossier.Resources
{
    public class ImageCache
    {
        private static readonly Dictionary<Uri, BitmapImage> _cache = new Dictionary<Uri, BitmapImage>();

        public static BitmapImage GetBitmapImage(Uri uriSource)
        {
            if (!_cache.ContainsKey(uriSource))
            {
                BitmapImage bitmapImage = null;
                try
                {
                    bitmapImage = new BitmapImage(uriSource);
                }
                catch (Exception) { } 

                _cache.Add(uriSource, bitmapImage);
            }
            return _cache[uriSource];
        }
    }
}
