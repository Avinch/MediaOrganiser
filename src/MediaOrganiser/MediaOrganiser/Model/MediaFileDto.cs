using System.Collections.Generic;

namespace MediaOrganiser.Model
{
    class MediaFileDto
    {
        public string Path { get; set; }
        public int FileTypeId { get; set; }
        public List<string> Categories { get; set; }
    }
}
