﻿using System;
using SmartStore.App.Enums;
using Xamarin.Forms;

namespace SmartStore.App.Models
{
    public class MenuItemModel : BindableObject
    {
        private string _title;
        private MenuItemType _menuItemType;
        private Type _viewModelType;
        private bool _isEnabled;

        public string Title
        {
            get => _title;

            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public MenuItemType MenuItemType
        {
            get => _menuItemType;

            set
            {
                _menuItemType = value;
                OnPropertyChanged();
            }
        }

        public Type ViewModelType
        {
            get => _viewModelType;

            set
            {
                _viewModelType = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;

            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
