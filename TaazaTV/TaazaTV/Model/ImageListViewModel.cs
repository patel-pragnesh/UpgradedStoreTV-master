using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TaazaTV.Model
{
    public class ImageListViewModel
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ImageSource ImgSource { get; set; }
    }

    public class VideoListViewModel
    {
        public string Name { get; set; }
        public byte[] Video { get; set; }
        public ImageSource ImgSource { get; set; }
    }
}
