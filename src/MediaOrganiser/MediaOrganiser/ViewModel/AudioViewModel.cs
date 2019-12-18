using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using MediaOrganiser.Data;
using MediaOrganiser.Messages;
using MediaOrganiser.Model;
using MediaOrganiser.Service;

namespace MediaOrganiser.ViewModel
{
    public class AudioViewModel : BaseFileViewModel<AudioFile>, INotifyPropertyChanged
    {
        public AudioViewModel() : base() { }

        public override List<Playlist<AudioFile>> SelectAllPlaylists()
        {
            return Repo.SelectAllAudioPlaylists();
        }

        public override List<AudioFile> SelectAllFiles()
        {
            return Repo.SelectAllAudioFiles();
        }
    }
}
