using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = TagLib.File;

namespace MediaOrganiser.Model
{
    abstract class MediaFile
    {
        protected readonly TagLib.File _tagFile;

        protected MediaFile(string path)
        {
            Path = path;

            _tagFile = File.Create(path);

            Length = _tagFile.Length;
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private long _length;

        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }
}
