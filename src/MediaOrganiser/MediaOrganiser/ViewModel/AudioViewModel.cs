using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MediaOrganiser.Model;

namespace MediaOrganiser.ViewModel
{
    public class AudioViewModel : BaseFileViewModel<AudioFile>, INotifyPropertyChanged
    {
        public override List<Playlist<AudioFile>> SelectAllPlaylists()
        {
            return Repo.SelectAllAudioPlaylists();
        }

        public override List<AudioFile> SelectAllFiles()
        {
            return Repo.SelectAllAudioFiles().OrderBy(x => x.Title).ToList();
        }

        public override void CreateBasePlaylist()
        {
            PlaylistService.CreateAudioPlaylist("Untitled playlist");
        }
    }
}
