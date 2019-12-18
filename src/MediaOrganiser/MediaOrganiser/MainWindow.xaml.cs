using System.ComponentModel;
using System.Windows;
using MediaOrganiser.Service;

namespace MediaOrganiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileScannerService _fileScannerService;
        private readonly PlaylistService _playlistService;
        private readonly CategoryService _categoryService;
        private readonly ConfigurationService _configurationService;

        public MainWindow()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _configurationService = new ConfigurationService();
                _configurationService.LoadConfigurationIntoMemory();
                _configurationService.ApplyApplicationWideSettings();
            }

            InitializeComponent();

            _fileScannerService = new FileScannerService();
            _playlistService = new PlaylistService();
            _categoryService = new CategoryService();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _fileScannerService.StartScan();
                _playlistService.LoadPlaylistsIntoMemory();
                _categoryService.LoadCategoriesIntoMemory();
            }
        }
    }
}
