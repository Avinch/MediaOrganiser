using System.Windows.Controls;
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
