using System.Linq;

namespace MediaOrganiser.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] AudioFileExtensions = {"mp3", "flac", "m4a", "ogg", "wav"};
        private static readonly string[] VideoFileExtensions = {"mp4", "mov", "mpg", "flv", "webm", "gifv", "wmv" };

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
