using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Model
{
    public class AudioFile : MediaFile
    {
        public AudioFile(string path) : base(path)
        {
            Title = _tagFile.Tag.Title;
            Artist = _tagFile.Tag.JoinedPerformers;
            Album = _tagFile.Tag.Album;
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _artist;

        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        private string _album;

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }




    }
}
