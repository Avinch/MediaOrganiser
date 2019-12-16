using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Model
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public List<string> FilePaths { get; set; }
    }
}
