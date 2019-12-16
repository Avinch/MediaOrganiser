using System.Collections.Generic;

namespace MediaOrganiser.Model
{
    public class Playlist<T> where T : MediaFile
    {
        public Playlist(int id, string name)
        {
            Id = id;
            Name = name;

            Items = new List<T>();

            if (typeof(T) == typeof(AudioFile))
            {
                Type = DataEnums.PlaylistType.Audio;
            }
            else if (typeof(T) == typeof(VideoFile))
            {
                Type = DataEnums.PlaylistType.Video;
            }
        }

        
        private DataEnums.PlaylistType _type;
        /// <summary>
        /// Only to be used when determining type of playlist on application
        /// load, as saving JSON doesn't store the type of T
        /// </summary>
        public DataEnums.PlaylistType Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        private List<T> _items;
        public List<T> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}
