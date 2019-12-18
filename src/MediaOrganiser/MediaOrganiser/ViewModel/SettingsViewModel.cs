using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganiser.Data;
using MediaOrganiser.Model;
using ModernWpf;

namespace MediaOrganiser.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly DataRepository _repo;

        public SettingsViewModel()
        {
            _repo = new DataRepository();

            _configuration = _repo.SelectConfiguration();
            UseDarkTheme = _configuration.UseDarkTheme;
        }

        private Configuration _configuration;
        public Configuration Configuration
        {
            get { return _configuration; }
            set { _configuration = value; OnPropertyChanged();}
        }

        private bool _useDarkTheme;
        public bool UseDarkTheme
        {
            get { return _useDarkTheme; }
            set { _useDarkTheme = value; OnPropertyChanged();
                Configuration.UseDarkTheme = value;
                SetApplicationTheme();
            }
        }

        private void SetApplicationTheme()
        {
            var theme = ApplicationTheme.Light;
            if (UseDarkTheme)
            {
                theme = ApplicationTheme.Dark;
            }

            ThemeManager.Current.ApplicationTheme = theme;
        }
    }
}
