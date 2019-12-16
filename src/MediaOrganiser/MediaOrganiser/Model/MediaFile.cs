using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = TagLib.File;

namespace MediaOrganiser.Model
{
    public abstract class MediaFile
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

        private List<Category> _categories;

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }

    }
}
