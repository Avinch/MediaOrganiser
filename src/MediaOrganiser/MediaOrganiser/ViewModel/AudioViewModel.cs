using System.Collections.ObjectModel;
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


        public AudioViewModel()
        {
            _repo = new DataRepository();

            MessengerService.Default.Register<FileScanCompleteMessage>(this, ScanCompleteReceived);
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
            set { _selectedFile = value; OnPropertyChanged(); }
        }

        private void ScanCompleteReceived(FileScanCompleteMessage obj)
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
        }
    }
}
