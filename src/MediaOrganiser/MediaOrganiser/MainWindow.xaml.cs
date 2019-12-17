using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaOrganiser.Service;

namespace MediaOrganiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PlaylistService _playlistService;
        private FileScannerService _fileScanService;

        public MainWindow()
        {
            _playlistService = new PlaylistService();
            _fileScanService = new FileScannerService();

            //_fileScanService.StartScan();

            //_playlistService.LoadPlaylistsIntoMemory();

            InitializeComponent();
        }
    }
}
