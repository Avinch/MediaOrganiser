using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
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

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived);

            ScanInProgress = false;
            Start();
        }

        private bool _scanInProgress;
        public bool ScanInProgress
        {
            get { return _scanInProgress; }
            set { _scanInProgress = value; OnPropertyChanged(); }
        }


        private void ScanCompleteReceived(FileScanCompleteMessage obj)
        {
            ScanInProgress = false;
        }

        private async void StartScan()
        {
            ScanInProgress = true;
            await _scannerService.StartScan();
        }

        private async void Start()
        {
             _scannerService.StartScan();
            _playlistService.LoadPlaylistsIntoMemory();
        }
    }
}
