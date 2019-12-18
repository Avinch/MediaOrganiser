using System.Collections.Generic;

namespace MediaOrganiser.Model
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public List<string> FilePaths { get; set; }
        public string Description { get; set; }
    }
}
