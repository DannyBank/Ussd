namespace Dbank.Digisoft.Config.Abstractions
{
    public interface IFileHelper
    {
        (bool, DirectoryInfo?) CreateDirectory(string path, string dirName);
        bool DeleteDirectory(string path, string dirName);
        bool DeleteFile(string path, string fileName);
        (bool Success, string? Type, Dictionary<string, string>? Contents) GetContents(string path);
        bool RenameDirectory(string path, string dirName, string newName);
        bool RenameFile(string path, string fileName, string newFilename);
    }
}
