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
    public class AssetLoader
    {
        public GameObject GetGameObject(string dataFolderPath, string label)
        {
            CheckDirectory(dataFolderPath);

            var configPath = GetJsonFilePath(dataFolderPath);

            CheckConfigExist(configPath);
            SetLoadPath(dataFolderPath);

            var locator = GetResourceLocator(configPath);
            var location = GetGameObjectLocation(locator, label);
            return LoadGameObject(location);
        }

        public IList<GameObject> GetGameObjects(string dataFolderPath, string label)
        {
            CheckDirectory(dataFolderPath);

            var configPath = GetJsonFilePath(dataFolderPath);

            CheckConfigExist(configPath);
            SetLoadPath(dataFolderPath);

            var locator = GetResourceLocator(configPath);
            var locations = GetGameObjectsLocations(locator, label);
            return LoadGameObjects(locations);
        }

        public ScriptableObject GetScriptableObject(string dataFolderPath, string label)
        {
            CheckDirectory(dataFolderPath);

            var configPath = GetJsonFilePath(dataFolderPath);

            CheckConfigExist(configPath);
            SetLoadPath(dataFolderPath);

            var locator = GetResourceLocator(configPath);
            var location = GetScriptableObjectLocation(locator, label);
            return LoadScriptableObject(location);
        }

        public IList<ScriptableObject> GetScriptableObjects(string dataFolderPath, string label)
        {
            CheckDirectory(dataFolderPath);

            var configPath = GetJsonFilePath(dataFolderPath);

            CheckConfigExist(configPath);
            SetLoadPath(dataFolderPath);

            var locator = GetResourceLocator(configPath);
            var location = GetScriptableObjectsLocations(locator, label);
            return LoadScriptableObjects(location);
        }

        private GameObject LoadGameObject(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(location);
            handle.WaitForCompletion();

            var result = handle.Result;
            if (result == null)
                throw new Exception();

            return result;
        }

        private IList<GameObject> LoadGameObjects(IList<IResourceLocation> locations)
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>(locations, null);
            handle.WaitForCompletion();

            var result = handle.Result;
            if (result == null || result.Count == 0)
                throw new Exception();

            return result;
        }

        private ScriptableObject LoadScriptableObject(IResourceLocation location)
        {
            var handle = Addressables.LoadAssetAsync<ScriptableObject>(location);
            handle.WaitForCompletion();

            var result = handle.Result;
            if (result == null)
                throw new Exception();

            return result;
        }

        private IList<ScriptableObject> LoadScriptableObjects(IList<IResourceLocation> locations)
        {
            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(locations, null);
            handle.WaitForCompletion();

            var result = handle.Result;
            if (result == null || result.Count == 0)
                throw new Exception();

            return result;
        }

        private IResourceLocation GetGameObjectLocation(IResourceLocator locator, string key)
        {
            return GetGameObjectsLocations(locator, key)[0];
        }

        private IList<IResourceLocation> GetGameObjectsLocations(IResourceLocator locator, string key)
        {
            var isFound = locator.Locate(
                key,
                typeof(GameObject),
                out IList<IResourceLocation> locations);

            if (isFound == false)
                throw new Exception();

            return locations;
        }

        private IResourceLocation GetScriptableObjectLocation(IResourceLocator locator, string key)
        {
            return GetScriptableObjectsLocations(locator, key)[0];
        }

        private IList<IResourceLocation> GetScriptableObjectsLocations(IResourceLocator locator, string key)
        {
            var isFound = locator.Locate(
                key,
                typeof(ScriptableObject),
                out IList<IResourceLocation> locations);

            if (isFound == false)
                throw new Exception();

            return locations;
        }

        private IResourceLocator GetResourceLocator(string calalogFilePath)
        {
            var handle = Addressables.LoadContentCatalogAsync(calalogFilePath);
            handle.WaitForCompletion();

            var result = handle.Result;
            if (result == null)
                throw new Exception();

            Addressables.Release(handle);
            return result;
        }

        private void SetLoadPath(string path)
        {
            Settings.LoadPath = path;
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