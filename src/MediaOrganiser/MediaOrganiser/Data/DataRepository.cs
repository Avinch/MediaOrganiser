using System;
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

        public List<string> SelectAllLibraries()
        {
            return DataStore.Instance.Configuration.Libraries;
        }

        public void AddLibrary(string libraryPath)
        {
            DataStore.Instance.Configuration.Libraries.Add(libraryPath);
        }

        public void RemoveLibrary(string libraryPath)
        {
            var foundLibrary = DataStore.Instance.Configuration.Libraries
                .SingleOrDefault(x => x == libraryPath);

            if (foundLibrary != null)
            {
                DataStore.Instance.Configuration.Libraries.Remove(foundLibrary);
            }
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
    }
}
