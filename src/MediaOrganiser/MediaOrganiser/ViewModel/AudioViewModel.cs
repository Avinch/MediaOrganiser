using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using MediaOrganiser.Data;
using MediaOrganiser.Messages;
using MediaOrganiser.Model;
using MediaOrganiser.Service;

namespace MediaOrganiser.ViewModel
{
    public class AudioViewModel : BaseViewModel
    {
        private readonly DataRepository _repo;

        public AudioViewModel()
        {
            _repo = new DataRepository();

            MessengerService.Default.Register<PlaylistsLoadedMessage>(this, PlaylistsLoadedReceived, MessageContexts.PopulateAudioPlaylists);

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived, MessageContexts.PopulateAudioFiles);

            ShownFiles = new BindingList<AudioFile>();

            AvailablePlaylists = new List<Playlist<AudioFile>>();
            AllFilesPlaylist = new Playlist<AudioFile>(999, "All", showPrefix: false);

            ReloadPlaylists();

            
            CountText = "None";
            DetailsPanelVisible = Visibility.Collapsed;
        }

        private void PlaylistsLoadedReceived(PlaylistsLoadedMessage obj)
        {
            ReloadPlaylists();
        }

        private void ReloadPlaylists()
        {
            AvailablePlaylists.Clear();
            AvailablePlaylists.Add(AllFilesPlaylist);
            AvailablePlaylists.AddRange(_repo.SelectAllAudioPlaylists());
        }

        private BindingList<AudioFile> _shownFiles;
        public BindingList<AudioFile> ShownFiles
        {
            get { return _shownFiles; }
            set { _shownFiles = value; OnPropertyChanged(); }
        }

        private AudioFile _selectedFile;
        public AudioFile SelectedFile
        {
            get { return _selectedFile; }
            set { _selectedFile = value; OnPropertyChanged(); SetDetailsPanelVisibility();}
        }

        private List<Playlist<AudioFile>> _availablePlaylists;
        public List<Playlist<AudioFile>> AvailablePlaylists
        {
            get { return _availablePlaylists; }
            set { _availablePlaylists = value; OnPropertyChanged(); }
        }

        private Playlist<AudioFile> _selectedPlaylist;
        public Playlist<AudioFile> SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set { _selectedPlaylist = value; OnPropertyChanged();
                SetItemsBasedOnView();
            }
        }

        private Playlist<AudioFile> _allFilesPlaylist;

        public Playlist<AudioFile> AllFilesPlaylist
        {
            get { return _allFilesPlaylist; }
            set { _allFilesPlaylist = value; OnPropertyChanged(); }
        }

        private void SetItemsBasedOnView()
        {
            if (SelectedPlaylist != null)
            {
                ReloadFiles(SelectedPlaylist.Items);
            }
        }

        private Visibility _detailsPanelVisible;

        public Visibility DetailsPanelVisible
        {
            get { return _detailsPanelVisible; }
            set { _detailsPanelVisible = value; OnPropertyChanged(); }
        }

        private void SetDetailsPanelVisibility()
        {
            if (SelectedFile != null)
            {
                DetailsPanelVisible = Visibility.Visible;
            }
            else
            {
                DetailsPanelVisible = Visibility.Collapsed;
            }
        }

        private string _countText;

        public string CountText
        {
            get { return _countText; }
            set { _countText = value; OnPropertyChanged();}
        }

        private void ScanCompleteReceived(FileScanCompleteMessage obj)
        {
            SelectedPlaylist = null;
            AllFilesPlaylist.Items = _repo.SelectAllAudioFiles();
            SelectedPlaylist = AllFilesPlaylist;
        }

        private void ReloadFiles(List<AudioFile> files)
        {
            ShownFiles.Clear();

            foreach (var result in files)
            {
                ShownFiles.Add(result);
            }

            CountText = $"Count: {ShownFiles.Count}";
        }
    }
}
