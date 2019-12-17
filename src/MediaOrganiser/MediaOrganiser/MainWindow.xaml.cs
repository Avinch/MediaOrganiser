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
        public MainWindow()
        { 
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                new FileScannerService().StartScan();
                new PlaylistService().LoadPlaylistsIntoMemory();
            }
        }
    }
}
