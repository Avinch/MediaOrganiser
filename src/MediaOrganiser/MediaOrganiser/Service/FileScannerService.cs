using System.IO;
using System.Linq;
using MediaOrganiser.Data;
using MediaOrganiser.Extensions;
using MediaOrganiser.Messages;

namespace MediaOrganiser.Service
{
    public class FileScannerService
    {
        private DataRepository _repo;

        public FileScannerService()
        {
            _repo = new DataRepository();
        }

        public void StartScan()
        {
            var librariesToScan = _repo.SelectAllLibraries();

            foreach (var library in librariesToScan)
            {
                var allFiles = Directory.GetFiles(library, "*.*", SearchOption.AllDirectories);

                foreach (var path in allFiles.Where(x => x.PathIsAudioFile()))
                {
                    _repo.AddAudioFile(path);
                }

                foreach (var path in allFiles.Where(x => x.PathIsVideoFile()))
                {
                    _repo.AddVideoFile(path);
                }
            }

            MessengerService.Default.Send(new FileScanCompleteMessage());
        }
    }
}
