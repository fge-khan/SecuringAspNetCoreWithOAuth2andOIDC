﻿using ImageGallery.Client.Controllers;
using ImageGallery.Model;
using System.Collections.Generic;

namespace ImageGallery.Client.ViewModels
{
    public class GalleryIndexViewModel
    {
        public IEnumerable<Image> Images { get; private set; }

        public GalleryIndexViewModel(IEnumerable<Image> images)
        {
           Images = images;
        }
    }
}
