using System.IO;
using UnityEngine;

namespace ExternalResourceLoader
{
    public static class Settings
    {
        public static string DataFolderName => "Data";
        public static string ResourcesPath => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "ExternalResources");
        public static string BuildPath => Path.Combine(ResourcesPath, DataFolderName);
        public static string LoadPath { get; set; } = Path.Combine(ResourcesPath, DataFolderName);
    }
}