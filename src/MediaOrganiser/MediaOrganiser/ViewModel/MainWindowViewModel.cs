using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MediaOrganiser.Messages;
using MediaOrganiser.Service;

namespace MediaOrganiser.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {

        public ICommand StartScanCommand;
        private readonly FileScannerService _scannerService;

        public MainWindowViewModel()
        {
            StartScanCommand = new RelayCommand(StartScan);
            _scannerService = new FileScannerService();

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived);

            StartScan();
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

        private void StartScan()
        {
            ScanInProgress = true;
            _scannerService.StartScan();
        }
    }
}
