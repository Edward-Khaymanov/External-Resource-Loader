using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ExternalResourceLoader
{
    public class ExternalAssetLoader
    {
        public GameObject GetGameObject(string dataFolderPath, string label)
        {
            CheckDirectory(dataFolderPath);

            var assetLoader = new ExternalAssetLoader();
            var configPath = assetLoader.GetJsonFilePath(dataFolderPath);

            CheckConfigExist(configPath);
            SetLoadPath(dataFolderPath);

            var locator = assetLoader.GetResourceLocator(configPath);
            var key = assetLoader.GetGameObjectKey(locator, label);
            return assetLoader.LoadGameobject(key);
        }

        public ScriptableObject GetScriptableObject(string dataFolderPath, string label)
        {
            CheckDirectory(dataFolderPath);

            var assetLoader = new ExternalAssetLoader();
            var configPath = assetLoader.GetJsonFilePath(dataFolderPath);

            CheckConfigExist(configPath);
            SetLoadPath(dataFolderPath);

            var locator = assetLoader.GetResourceLocator(configPath);
            var key = assetLoader.GetScriptableObjectKey(locator, label);
            return assetLoader.LoadScriptableObject(key);
        }

        public void SetLoadPath(string path)
        {
            Settings.LoadPath = path;
        }

        private GameObject LoadGameobject(string addressableKey)
        {
            var mapHandle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
            mapHandle.WaitForCompletion();

            var result = mapHandle.Result;
            if (result == null)
                throw new Exception();

            return result;
        }

        private ScriptableObject LoadScriptableObject(string addressableKey)
        {
            var mapHandle = Addressables.LoadAssetAsync<ScriptableObject>(addressableKey);
            mapHandle.WaitForCompletion();

            var result = mapHandle.Result;
            if (result == null)
                throw new Exception();

            return result;
        }

        private IResourceLocator GetResourceLocator(string calalogFilePath)
        {
            var catalogHandle = Addressables.LoadContentCatalogAsync(calalogFilePath);
            catalogHandle.WaitForCompletion();

            var result = catalogHandle.Result;
            if (result == null)
                throw new Exception();

            Addressables.Release(catalogHandle);
            return result;
        }

        private string GetGameObjectKey(IResourceLocator locator, string label)
        {
            var mapIsFound = locator.Locate(
                label,
                typeof(GameObject),
                out IList<IResourceLocation> locations);

            if (mapIsFound == false)
                throw new Exception();

            var key = locations[0].PrimaryKey;
            if (string.IsNullOrEmpty(key))
                throw new Exception();

            return key;
        }

        private string GetScriptableObjectKey(IResourceLocator locator, string label)
        {
            var isFound = locator.Locate(
                label,
                typeof(ScriptableObject),
                out IList<IResourceLocation> locations);

            if (isFound == false)
                throw new Exception();

            var key = locations[0].PrimaryKey;
            if (string.IsNullOrEmpty(key))
                throw new Exception();

            return key;
        }

        private string GetJsonFilePath(string folderPath)
        {
            return Directory.GetFiles(folderPath).FirstOrDefault(x => x.EndsWith(".json"));
        }

        private void CheckDirectory(string path)
        {
            var directoryExist = Directory.Exists(path);
            if (directoryExist == false)
                throw new DirectoryNotFoundException();
        }

        private void CheckConfigExist(string path)
        {
            var exist = File.Exists(path);
            if (exist == false)
                throw new FileNotFoundException();
        }
    }
}