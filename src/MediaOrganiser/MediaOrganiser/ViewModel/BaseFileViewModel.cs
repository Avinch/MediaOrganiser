﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MediaOrganiser.Annotations;
using MediaOrganiser.Data;
using MediaOrganiser.Messages;
using MediaOrganiser.Model;
using MediaOrganiser.Service;

namespace MediaOrganiser.ViewModel
{
    public abstract class BaseFileViewModel<T> where T : MediaFile, INotifyPropertyChanged
    {
        protected readonly DataRepository Repo;
        protected readonly PlaylistService PlaylistService;
        private readonly CategoryService _categoryService;

        public ICommand AddPlaylistCommand { get; set; }
        public ICommand AddFileToPlaylistCommand { get; set; }
        public ICommand OpenSelectedFileCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand ClearCategoriesCommand { get; set; }

        protected BaseFileViewModel()
        {
            Repo = new DataRepository();
            PlaylistService = new PlaylistService();
            _categoryService = new CategoryService();

            MessengerService.Default.Register<PlaylistsLoadedMessage>(this, PlaylistsLoadedReceived, MessageContexts.PlaylistsLoaded);

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived, MessageContexts.FileScanComplete); 
            
            MessengerService.Default.Register<FileCategoriesUpdatedMessage>(this,
                CategoriesUpdatedReceived, MessageContexts.FileCategoriesUpdatedMessage);

            ShownFiles = new BindingList<T>();
            ContextMenuItems = new ObservableCollection<MenuItemViewModel>();

            AvailablePlaylists = new BindingList<Playlist<T>>();
            AllFilesPlaylist = new Playlist<T>((int)DataEnums.PredefinedPlaylists.AllFiles, "All", showPrefix: false);

            ReloadPlaylists();

            AddPlaylistCommand = new RelayCommand(CreateNewPlaylist);
            AddFileToPlaylistCommand = new RelayCommand<int>(AddFileToPlaylist);
            OpenSelectedFileCommand = new RelayCommand(OpenSelectedFile);
            AddCategoryCommand = new RelayCommand(AddCategory);
            ClearCategoriesCommand = new RelayCommand(ClearCategories);

            CountText = "None";
            FileDetailsPanelVisible = Visibility.Collapsed;
        }

        private void CategoriesUpdatedReceived(FileCategoriesUpdatedMessage obj)
        {
            ReloadPlaylists();
        }

        private void ClearCategories()
        {
            SelectedFile.Categories.Clear();
            SetSelectedFileCategoriesDisplayText();
            SelectedPlaylist = AllFilesPlaylist;
            ReloadPlaylists();
        }

        private void AddCategory()
        {
            if (AddCategoryInput == null)
            {
                return;
            }

            if (SelectedFile.Categories == null)
            {
                SelectedFile.Categories = new List<string>();
            }
            SelectedFile.Categories.Add(AddCategoryInput);
            SetSelectedFileCategoriesDisplayText();
            AddCategoryInput = null;
            ReloadPlaylists();
        }

        private void AddFileToPlaylist(int playlistId)
        {
            var playlistToAddTo = AvailablePlaylists.Single(x => x.Id == playlistId);

            playlistToAddTo.Items.Add(SelectedFile);
        }

        private void CreateNewPlaylist()
        {
            CreateBasePlaylist();
            ReloadPlaylists();
        }

        /// <summary>
        /// Must be implemented by child object
        /// </summary>
        public virtual void CreateBasePlaylist()
        {
            throw new NotImplementedException();
        }

        private void OpenSelectedFile()
        {
            Process.Start(SelectedFile.Path);
        }

        private void PlaylistsLoadedReceived(PlaylistsLoadedMessage obj)
        {
            ReloadPlaylists();
        }

        /// <summary>
        /// Must be implemented by child object
        /// </summary>
        /// <returns></returns>
        public virtual List<Playlist<T>> SelectAllPlaylists()
        {
            throw new NotImplementedException();
        }

        private void ReloadPlaylists()
        {
            AvailablePlaylists.Clear();
            AvailablePlaylists.Add(AllFilesPlaylist);

            foreach (var list in SelectAllPlaylists())
            {
                AvailablePlaylists.Add(list);
            }

            foreach (var category in GetAllUniqueAvailableCategories())
            {
                AvailablePlaylists.Add(
                    new Playlist<T>(
                        (int)DataEnums.PredefinedPlaylists.Category,
                        $"Category: {category}", false)
                    {
                        Items = AllFilesPlaylist.Items.Where(x => x.Categories.Contains(category)).ToList()
                    });
            }

            GenerateContextMenu();
        }

        private List<string> GetAllUniqueAvailableCategories()
        {
            var allCategories = new List<string>();

            foreach (var category in AllFilesPlaylist.Items.Select(x => x.Categories))
            {
                if (category == null) { continue; }
                allCategories.AddRange(category);
            }

            return allCategories.Distinct().ToList();
        }

        private BindingList<T> _shownFiles;
        public BindingList<T> ShownFiles
        {
            get { return _shownFiles; }
            set { _shownFiles = value; OnPropertyChanged(); }
        }

        private T _selectedFile;
        public T SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value; OnPropertyChanged(); SetDetailsPanelVisibility(); SetSelectedFileCategoriesDisplayText();
                AddCategoryInput = null;
            }
        }

        private BindingList<Playlist<T>> _availablePlaylists;
        public BindingList<Playlist<T>> AvailablePlaylists
        {
            get { return _availablePlaylists; }
            set { _availablePlaylists = value; OnPropertyChanged(); }
        }

        private Playlist<T> _selectedPlaylist;
        public Playlist<T> SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                _selectedPlaylist = value; OnPropertyChanged();
                SelectedPlaylistChanged();
                SetPlaylistDetailsVisibility();
            }
        }

        private Playlist<T> _allFilesPlaylist;

        public Playlist<T> AllFilesPlaylist
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

            PlaylistDetailsVisible =
                !(SelectedPlaylist.Id == (int)DataEnums.PredefinedPlaylists.AllFiles
                    || SelectedPlaylist.Id == (int)DataEnums.PredefinedPlaylists.Category)
                    ? Visibility.Visible : Visibility.Collapsed;
        }

        private string _countText;

        public string CountText
        {
            get { return _countText; }
            set { _countText = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Must be implemented by child object
        /// </summary>
        /// <returns></returns>
        public virtual List<T> SelectAllFiles()
        {
            throw new NotImplementedException();
        }

        private void ScanCompleteReceived(FileScanCompleteMessage obj)
        {
            _categoryService.LoadCategoriesIntoMemory();
            SelectedPlaylist = null;
            AllFilesPlaylist.Items = SelectAllFiles(); 
            SelectedPlaylist = AllFilesPlaylist;
            ReloadPlaylists();
        }

        private void ReloadFiles(List<T> files)
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
                if (playlist.Id == (int)DataEnums.PredefinedPlaylists.AllFiles
                    || playlist.Id == (int)DataEnums.PredefinedPlaylists.Category) { continue; }

                ContextMenuItems.Add(new MenuItemViewModel
                {
                    Text = playlist.Name,
                    Command = AddFileToPlaylistCommand,
                    BaseObject = playlist
                });
            }

            MessengerService.Default.Send(new PlaylistContextMenuItemsGenerated(), MessageContexts.PlaylistContextMenuItemsGenerated);
        }

        private string _selectedFileCategories;

        public string SelectedFileCategories
        {
            get { return _selectedFileCategories; }
            set { _selectedFileCategories = value; OnPropertyChanged(); }
        }

        private void SetSelectedFileCategoriesDisplayText()
        {
            if (SelectedFile?.Categories == null || SelectedFile?.Categories.Count == 0)
            {
                SelectedFileCategories = "None";
            }
            else
            {
                SelectedFileCategories = string.Join(", ", SelectedFile.Categories);
            }
        }

        private string _addCategoryInput;
        public string AddCategoryInput
        {
            get { return _addCategoryInput; }
            set { _addCategoryInput = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}