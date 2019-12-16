using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Service
{
    public class SettingsService
    {
        private static SettingsService _instance;

        public static SettingsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsService();
                }

                return _instance;
            }
        }

        private const string PlaylistFileName = "playlists.json";

        public string GetSettingsFolder()
        {
            return $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MediaOrganiser";
        }

        public string GetPlaylistFilePath()
        {
            return $@"{GetSettingsFolder()}\{PlaylistFileName}";
        }
    }
}
