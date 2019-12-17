using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class AudioViewModel : BaseViewModel
    {
        private readonly DataRepository _repo;
        private readonly PlaylistService _playlistService;
        public ICommand AddPlaylistCommand { get; set; }
        public ICommand AddFileToPlaylistCommand { get; set; }

        public AudioViewModel()
        {
            _repo = new DataRepository();
            _playlistService = new PlaylistService();

            MessengerService.Default.Register<PlaylistsLoadedMessage>(this, PlaylistsLoadedReceived, MessageContexts.PopulateAudioPlaylists);

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived, MessageContexts.PopulateAudioFiles);

            ShownFiles = new BindingList<AudioFile>();
            ContextMenuItems = new ObservableCollection<MenuItemViewModel>();

            AvailablePlaylists = new BindingList<Playlist<AudioFile>>();
            AllFilesPlaylist = new Playlist<AudioFile>((int)DataEnums.PredefinedPlaylists.AllFiles, "All", showPrefix: false);

            ReloadPlaylists();

            AddPlaylistCommand = new RelayCommand(CreateNewPlaylist);
            AddFileToPlaylistCommand = new RelayCommand<int>(AddFileToPlaylist);

            CountText = "None";
            FileDetailsPanelVisible = Visibility.Collapsed;
        }

        private void AddFileToPlaylist(int playlistId)
        {
            var playlistToAddTo = AvailablePlaylists.Single(x => x.Id == playlistId);

            playlistToAddTo.Items.Add(SelectedFile);
        }

        private void CreateNewPlaylist()
        {
            _playlistService.CreateAudioPlaylist("Untitled playlist");
            ReloadPlaylists();
        }

        private void PlaylistsLoadedReceived(PlaylistsLoadedMessage obj)
        {
            ReloadPlaylists();
        }

        private void ReloadPlaylists()
        {
            AvailablePlaylists.Clear();
            AvailablePlaylists.Add(AllFilesPlaylist);
            foreach (var list in _repo.SelectAllAudioPlaylists())
            {
                AvailablePlaylists.Add(list);
            }
            GenerateContextMenu();
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

        private BindingList<Playlist<AudioFile>> _availablePlaylists;
        public BindingList<Playlist<AudioFile>> AvailablePlaylists
        {
            get { return _availablePlaylists; }
            set { _availablePlaylists = value; OnPropertyChanged(); }
        }

        private Playlist<AudioFile> _selectedPlaylist;
        public Playlist<AudioFile> SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set { _selectedPlaylist = value; OnPropertyChanged();
                SelectedPlaylistChanged();
                SetPlaylistDetailsVisibility();
            }
        }

        private Playlist<AudioFile> _allFilesPlaylist;

        public Playlist<AudioFile> AllFilesPlaylist
        {
            get { return _allFilesPlaylist; }
            set { _allFilesPlaylist = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MenuItemViewModel> _contextMenuItems;
        public ObservableCollection<MenuItemViewModel> ContextMenuItems
        {
            get { return _contextMenuItems; }
            set { _contextMenuItems = value; }
        }

        private Visibility _playlistDetailsVisible;
        public Visibility PlaylistDetailsVisible
        {
            get { return _playlistDetailsVisible; }
            set { _playlistDetailsVisible = value; OnPropertyChanged(); }
        }

        private void SelectedPlaylistChanged()
        {
            if (SelectedPlaylist != null)
            {
                ReloadFiles(SelectedPlaylist.Items);
            }
        }

        private Visibility _fileDetailsPanelVisible;

        public Visibility FileDetailsPanelVisible
        {
            get { return _fileDetailsPanelVisible; }
            set { _fileDetailsPanelVisible = value; OnPropertyChanged(); }
        }

        private void SetDetailsPanelVisibility()
        {
            if (SelectedFile != null)
            {
                FileDetailsPanelVisible = Visibility.Visible;
                GenerateContextMenu();
            }
            else
            {
                FileDetailsPanelVisible = Visibility.Collapsed;
            }
        }

        private void SetPlaylistDetailsVisibility()
        {
            if (SelectedPlaylist == null)
            {
                return;
            }

            PlaylistDetailsVisible = SelectedPlaylist.Id != (int) DataEnums.PredefinedPlaylists.AllFiles ? Visibility.Visible : Visibility.Collapsed;
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

        private void GenerateContextMenu()
        {
            ContextMenuItems.Clear();

            foreach (var playlist in AvailablePlaylists)
            {
                if (playlist.Id == (int)DataEnums.PredefinedPlaylists.AllFiles) { continue;}

                ContextMenuItems.Add(new MenuItemViewModel
                {
                    Text = playlist.Name,
                    Command = AddFileToPlaylistCommand,
                    BaseObject = playlist
                });
            }

            MessengerService.Default.Send(new PlaylistContextMenuItemsGenerated(), MessageContexts.PlaylistContextMenuItemsGenerated);
        }
    }
}
