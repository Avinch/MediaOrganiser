using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganiser.Model;

namespace MediaOrganiser.Data
{
    public class DataRepository
    {
        public DataRepository() { }

        public void SetConfiguration(Configuration configuration)
        {
            DataStore.Instance.Configuration = configuration;
        }

        public Configuration SelectConfiguration()
        {
            return DataStore.Instance.Configuration;
        }

        public string SelectLibrary()
        {
            return DataStore.Instance.Configuration.LibraryPath;
        }

        public void AddAudioFile(string path)
        {
            var file = new AudioFile(path);

            DataStore.Instance.AudioFiles.Add(file);
        }

        public void AddVideoFile(string path)
        {
            var file = new VideoFile(path);

            DataStore.Instance.VideoFiles.Add(file);
        }

        public void ReplaceAllAudioFiles(List<string> paths)
        {
            DataStore.Instance.AudioFiles.Clear();
            foreach (var path in paths)
            {
                var file = new AudioFile(path);

                DataStore.Instance.AudioFiles.Add(file);
            }
        }

        public void ReplaceAllVideoFiles(List<string> paths)
        {
            DataStore.Instance.VideoFiles.Clear();
            foreach (var path in paths)
            {
                var file = new VideoFile(path);

                DataStore.Instance.VideoFiles.Add(file);
            }
        }

        public List<AudioFile> SelectAllAudioFiles()
        {
            return DataStore.Instance.AudioFiles;
        }

        public List<VideoFile> SelectAllVideoFiles()
        {
            return DataStore.Instance.VideoFiles;
        }

        public AudioFile SelectAudioFileByPath(string path)
        {
            return DataStore.Instance.AudioFiles.SingleOrDefault(x => x.Path == path);
        }

        public VideoFile SelectVideoFileByPath(string path)
        {
            return DataStore.Instance.VideoFiles.SingleOrDefault(x => x.Path == path);
        }

        public void AddAudioPlaylist(Playlist<AudioFile> playlist)
        {
            DataStore.Instance.AudioPlaylists.Add(playlist);
        }

        public void AddVideoPlaylist(Playlist<VideoFile> playlist)
        {
            DataStore.Instance.VideoPlaylists.Add(playlist);
        }

        public List<Playlist<AudioFile>> SelectAllAudioPlaylists()
        {
            return DataStore.Instance.AudioPlaylists;
        }

        public List<Playlist<VideoFile>> SelectAllVideoPlaylists()
        {
            return DataStore.Instance.VideoPlaylists;
        }

        public void ClearAllPlaylists()
        {
            DataStore.Instance.VideoPlaylists.Clear();
            DataStore.Instance.AudioPlaylists.Clear();
        }
    }
}
