﻿using Newtonsoft.Json.Linq;

namespace Dbank.Digisoft.Config.Abstractions
{
    public interface IFileHelper
    {
        (bool, DirectoryInfo?) CreateDirectory(string path, string dirName);
        bool DeleteDirectory(string path, string dirName);
        bool DeleteFile(string path, string fileName);
        Task<List<JObject>?> GetContents(Dictionary<string, string> filePaths);
        (bool Success, string? Type, Dictionary<string, string>? DirectoriesAndFiles) 
            GetDirectoriesAndFiles(string environment);
        bool RenameDirectory(string path, string dirName, string newName);
        bool RenameFile(string path, string fileName, string newFilename);
    }
}
