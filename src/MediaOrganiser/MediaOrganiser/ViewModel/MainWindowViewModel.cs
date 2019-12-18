using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MediaOrganiser.Messages;
using MediaOrganiser.Service;

namespace MediaOrganiser.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {

        public ICommand StartScanCommand { get; set; }
        private readonly FileScannerService _scannerService;
        private readonly PlaylistService _playlistService;

        public MainWindowViewModel()
        {
            StartScanCommand = new RelayCommand(StartScan);
            _scannerService = new FileScannerService();
            _playlistService = new PlaylistService();

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived, MessageContexts.FileScanComplete);
            MessengerService.Default.Register<FileScanStartedMessage>(this, ScanStartReceived, MessageContexts.FileScanStarted);

            ScanInProgress = false;
        }

        private void ScanStartReceived(FileScanStartedMessage obj)
        {
            ScanInProgress = true;
        }

        private bool _tabControlEnabled;

        public bool TabControlEnabled
        {
            get { return _tabControlEnabled; }
            set { _tabControlEnabled = value; OnPropertyChanged(); }
        }

        private bool _scanInProgress;
        public bool ScanInProgress
        {
            get { return _scanInProgress; }
            set { _scanInProgress = value; OnPropertyChanged();
                TabControlEnabled = !_scanInProgress;
            }
        }

        private void ScanCompleteReceived(FileScanCompleteMessage obj)
        {
            ScanInProgress = false;
            UpdateLastSyncStatus();
        }

        private async void StartScan()
        {
            ScanInProgress = true;
            _playlistService.SavePlaylistsToFile();
            await _scannerService.StartScanAsync();
            _playlistService.LoadPlaylistsIntoMemory();
        }

        private string _lastSyncStatus;

        public string LastSyncStatus
        {
            get { return _lastSyncStatus; }
            set { _lastSyncStatus = value; OnPropertyChanged(); }
        }

        private void UpdateLastSyncStatus()
        {
            LastSyncStatus = $"Last sync: {DateTime.Now}";
        }
    }
}
