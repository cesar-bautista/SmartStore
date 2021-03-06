﻿using System;

namespace SmartStore.App.Models
{
    public class MenuModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public Type ViewModelType { get; set; }

        public bool IsEnabled { get; set; }
    }
}
