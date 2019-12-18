using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganiser.Data;
using MediaOrganiser.Model;
using ModernWpf;
using Newtonsoft.Json;

namespace MediaOrganiser.Service
{
    public class ConfigurationService
    {
        private readonly DataRepository _repo;

        public ConfigurationService()
        {
            _repo = new DataRepository();
        }


        public void LoadConfigurationIntoMemory()
        {
            if (!File.Exists(SettingsService.Instance.GetConfigurationPath()))
            {
                _repo.SetConfiguration(new Configuration());
                return;
            }

            var serialized = File.ReadAllText(SettingsService.Instance.GetConfigurationPath());

            _repo.SetConfiguration(JsonConvert.DeserializeObject<Configuration>(serialized));
        }

        public void ApplyApplicationWideSettings()
        {
            var conf = _repo.SelectConfiguration();

            var theme = ApplicationTheme.Light;
            if (conf.UseDarkTheme)
            {
                theme = ApplicationTheme.Dark;
            }

            ThemeManager.Current.ApplicationTheme = theme;
        }

        public void SaveConfigurationToFile()
        {
            var serialized = JsonConvert.SerializeObject(_repo.SelectConfiguration(), Formatting.Indented);

            if (!Directory.Exists(SettingsService.Instance.GetSettingsFolder()))
            {
                Directory.CreateDirectory(SettingsService.Instance.GetSettingsFolder());
            }

            File.WriteAllText(SettingsService.Instance.GetConfigurationPath(), serialized);
        }
    }
}
