using System;
using System.IO;

namespace ExternalResourceLoader
{
    public static class Settings
    {
        public static string DataFolderName = "Data";
        public static string ResourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalResources");
        public static string BuildPath = $"{ResourcesPath}\\{DataFolderName}";
        public static string LoadPath = $"{ResourcesPath}\\{DataFolderName}";
    }
}
