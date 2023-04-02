using System.IO;
using System.Linq;

namespace Dbank.Digisoft.PrediBet.Ussd.Data.Extensions
{
    public static class DirectoryExtensions
    {
        public static bool IsDirectoryEmpty(this string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
    }
}