using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Model
{
    public class VideoFile : MediaFile
    {
        public VideoFile(string path) : base(path)
        {
            Resolution = $"{TagFile.Properties.VideoWidth}x{TagFile.Properties.VideoHeight}";
            Title = TagFile.Tag.Title;
            Year = TagFile.Tag.Year;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private uint _year;
        public uint Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private string _resolution;
        public string Resolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }

    }
}
