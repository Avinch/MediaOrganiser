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
        private readonly PlaylistService _playlistService;
        private readonly CategoryService _categoryService;

        public App()
        {
            _playlistService = new PlaylistService();
            _categoryService = new CategoryService();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _playlistService.SavePlaylistsToFile();
            _categoryService.SaveCategoriesToFile();
        }
    }
}
