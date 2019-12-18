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
        private const string FilesFileName = "files.json";

        public string GetSettingsFolder()
        {
            return $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MediaOrganiser";
        }

        public string GetPlaylistPath()
        {
            return $@"{GetSettingsFolder()}\{PlaylistFileName}";
        }

        public string GetFilesPath()
        {
            return $@"{GetSettingsFolder()}\{FilesFileName}";
        }
    }
}
