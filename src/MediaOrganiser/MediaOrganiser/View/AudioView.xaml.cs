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
using MediaOrganiser.Messages;
using MediaOrganiser.Model;
using MediaOrganiser.Service;
using MediaOrganiser.ViewModel;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace MediaOrganiser.View
{
    /// <summary>
    /// Interaction logic for AudioView.xaml
    /// </summary>
    public partial class AudioView : UserControl
    {
        public AudioView()
        {
            MessengerService.Default.Register<PlaylistContextMenuItemsGenerated>(this, GenerateContextMenuItemsControls, MessageContexts.PlaylistContextMenuItemsGenerated);
            InitializeComponent();
        }

        private void GenerateContextMenuItemsControls(PlaylistContextMenuItemsGenerated obj)
        {
            var source = (AudioViewModel) DataContext;

            if (source == null)
            {
                return;

            }

            var flyout = new MenuFlyout();
            foreach (var item in source.ContextMenuItems)
            {
                flyout.Items.Add(new MenuItem
                {
                    Header = item.Text,
                    Command = item.Command,
                    CommandParameter = ((Playlist<AudioFile>)item.BaseObject).Id
                });
            }

            flyout.Placement = FlyoutPlacementMode.Bottom;

            AddToPlaylistButton.Flyout = flyout;
        }
    }
}
