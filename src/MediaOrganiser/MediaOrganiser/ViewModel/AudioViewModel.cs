using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived);

            Files = new ObservableCollection<AudioFile>();

            CountText = "None";
        }

        private ObservableCollection<AudioFile> _files;
        public ObservableCollection<AudioFile> Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged(); }
        }

        private AudioFile _selectedFile;
        public AudioFile SelectedFile
        {
            get { return _selectedFile; }
            set { _selectedFile = value; OnPropertyChanged(); SetDetailsPanelVisibility();}
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


        private async void ScanCompleteReceived(FileScanCompleteMessage obj)
        {
            ReloadFiles();
        }

        private void ReloadFiles()
        {
            Files.Clear();

            var results = _repo.SelectAllAudioFiles();

            foreach (var result in results)
            {
                Files.Add(result);
            }

            CountText = $"Count: {Files.Count}";
        }
    }
}
