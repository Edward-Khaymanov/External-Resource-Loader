using System.IO;
using System.Linq;
using System.Reflection;

namespace ExternalResourceLoader
{
    public class DllLoader
    {
        public Assembly Load(string path)
        {
            var assemblyBytes = File.ReadAllBytes(path);
            var assembly = Assembly.Load(assemblyBytes);
            return assembly;
        }

        public string GetDllFilePath(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.dll").FirstOrDefault();
        }
    }
}