using System.Collections.Generic;
using System.ComponentModel;
using MediaOrganiser.Model;

namespace MediaOrganiser.ViewModel
{
    public class VideoViewModel : BaseFileViewModel<VideoFile>, INotifyPropertyChanged
    {
        public override List<Playlist<VideoFile>> SelectAllPlaylists()
        {
            return Repo.SelectAllVideoPlaylists();
        }

        public override List<VideoFile> SelectAllFiles()
        {
            return Repo.SelectAllVideoFiles();
        }

        public override void CreateBasePlaylist()
        {
            PlaylistService.CreateVideoPlaylist("Untitled playlist");
        }
    }
}
