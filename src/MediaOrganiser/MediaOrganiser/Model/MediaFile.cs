using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MediaOrganiser.Annotations;
using TagLib;

namespace MediaOrganiser.Model
{
    public abstract class MediaFile : INotifyPropertyChanged
    {
        protected readonly File TagFile;

        protected MediaFile(string path)
        {
            Path = path;

            TagFile = File.Create(path);

            Length = TagFile.Properties.Duration;

            Categories = new List<string>();
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

        private List<string> _categories;
        public List<string> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
