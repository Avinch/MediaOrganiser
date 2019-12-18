using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Model
{
    class MediaFileDto
    {
        public string Path { get; set; }
        public int FileTypeId { get; set; }
        public List<string> Categories { get; set; }
    }
}
