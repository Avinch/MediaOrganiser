using System;
using System.Collections.Generic;
using System.Windows;
using TagLib;

namespace MediaOrganiser.Model
{
    public abstract class MediaFile
    {
        protected readonly File _tagFile;

        protected MediaFile(string path)
        {
            Path = path;

            _tagFile = File.Create(path);

            Length = _tagFile.Properties.Duration;
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private TimeSpan _length;

        public TimeSpan Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public string FormattedLength => Length.ToString(@"mm\:ss");

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
