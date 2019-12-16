using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganiser.Model;

namespace MediaOrganiser.Data
{
    internal class DataStore
    {
        private static DataStore _instance;

        public static DataStore Instance
        {
            get { if(_instance == null) { _instance = new DataStore();}

                return _instance;
            }
        }

        public Configuration Configuration;

        public List<AudioFile> AudioFiles;
        public List<VideoFile> VideoFiles;

        public List<Playlist<AudioFile>> AudioPlaylists;
        public List<Playlist<VideoFile>> VideoPlaylists;

        private DataStore()
        {
            AudioFiles = new List<AudioFile>();
            VideoFiles = new List<VideoFile>();

            AudioPlaylists = new List<Playlist<AudioFile>>();
            VideoPlaylists = new List<Playlist<VideoFile>>();
        }

    }
}
