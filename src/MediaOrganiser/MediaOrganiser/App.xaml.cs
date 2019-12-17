using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MediaOrganiser.Service;

namespace MediaOrganiser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private PlaylistService _playlistService;

        public App()
        {
            _playlistService = new PlaylistService();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _playlistService.SavePlaylistsToFile();
        }
    }
}
