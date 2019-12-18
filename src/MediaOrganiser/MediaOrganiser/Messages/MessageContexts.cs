using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Messages
{
    public enum MessageContexts
    {
        PopulateAudioPlaylists,
        PopulateAudioFiles,
        PopulateVideoPlaylists,
        PopulateVideoFiles,
        FileScanComplete,
        PlaylistContextMenuItemsGenerated,
        FileScanStarted,
        FileCategoriesUpdatedMessage
    }
}
