using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task StartScan()
        {
            var librariesToScan = _repo.SelectAllLibraries();

            foreach (var library in librariesToScan)
            {
                var allFiles = Directory.GetFiles(library, "*.*", SearchOption.AllDirectories);

                foreach (var path in allFiles.Where(x => x.PathIsAudioFile()))
                {
                    await Task.Run(() => _repo.AddAudioFile(path));
                }

                foreach (var path in allFiles.Where(x => x.PathIsVideoFile()))
                {
                    await Task.Run(() => _repo.AddVideoFile(path));
                }
            }

            MessengerService.Default.Send(new FileScanCompleteMessage());
        }
    }
}
