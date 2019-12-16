using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] AudioFileExtensions = {"mp3", "flac"};
        private static readonly string[] VideoFileExtensions = {"mp4", "mov" };

        public static bool PathIsAudioFile(this string path)
        {
            return AudioFileExtensions.Any(path.EndsWith);
        }

        public static bool PathIsVideoFile(this string path)
        {
            return VideoFileExtensions.Any(path.EndsWith);
        }
    }
}
