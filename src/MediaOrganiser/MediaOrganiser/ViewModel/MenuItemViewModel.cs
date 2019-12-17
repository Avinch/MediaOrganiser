﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MediaOrganiser.ViewModel
{
    public class MenuItemViewModel
    {
        public MenuItemViewModel()
        {
            MenuItems = new List<MenuItemViewModel>();
        }

        public string Text { get; set; }
        public ICommand Command { get; set; }
        public IList<MenuItemViewModel> MenuItems { get; private set; }
        public object BaseObject { get; set; }
    }
}
