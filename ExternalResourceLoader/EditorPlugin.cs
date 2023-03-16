#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ExternalResourceLoader
{
    public class EditorPlugin : EditorWindow
    {
        [SerializeField] private string _dllName = "Dll name";

        private const string LOCAL_BUILD_PATH_VARIABLE = "Local.BuildPath";
        private const string LOCAL_LOAD_PATH_VARIABLE = "Local.LoadPath";
        private const string MONOSCRIPT_BUNDLE_CUSTOM_NAMING = "_ExternalResource_";

        [MenuItem("Window/ExternalResourceLoader/Show")]
        public static void ShowWindow()
        {
            GetWindow<EditorPlugin>("External Resource Loader");
        }

        [MenuItem("Window/ExternalResourceLoader/Init")]
        public static void Init()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;

            var newProfileId = profileSettings.AddProfile("ExternalResources", settings.activeProfileId);
            settings.activeProfileId = newProfileId;

            profileSettings.SetValue(newProfileId, LOCAL_BUILD_PATH_VARIABLE, "[ExternalResourceLoader.Settings.BuildPath]");
            profileSettings.SetValue(newProfileId, LOCAL_LOAD_PATH_VARIABLE, "{ExternalResourceLoader.Settings.LoadPath}");

            settings.BuildRemoteCatalog = true;
            settings.DisableCatalogUpdateOnStartup = false;
            settings.RemoteCatalogBuildPath.SetVariableByName(settings, LOCAL_BUILD_PATH_VARIABLE);
            settings.RemoteCatalogLoadPath.SetVariableByName(settings, LOCAL_LOAD_PATH_VARIABLE);

            settings.UniqueBundleIds = true;
            settings.MonoScriptBundleNaming = UnityEditor.AddressableAssets.Build.MonoScriptBundleNaming.Custom;
            settings.MonoScriptBundleCustomNaming = MONOSCRIPT_BUNDLE_CUSTOM_NAMING;
        }

        private static void Build()
        {
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
        }

        private void OnGUI()
        {
            var buttonSettings = GUILayout.Height(20);

            var build = GUILayout.Button("Build", buttonSettings);
            var deleteBuild = GUILayout.Button("Delete build", buttonSettings);
            var openBuildFolder = GUILayout.Button("Open resources folder", buttonSettings);

            GUILayout.Space(10);

            _dllName = EditorGUILayout.TextField(_dllName);
            var copyDll = GUILayout.Button("Copy dll to resources folder", buttonSettings);
            if (copyDll)
            {
                var sourcePath = Path.Combine(Application.dataPath.Replace("Assets", @"Library\ScriptAssemblies"), $"{_dllName}.dll");
                var targetPath = Path.Combine(Settings.ResourcesPath, _dllName);
                if (File.Exists(sourcePath))
                    File.Copy(sourcePath, targetPath, true);
            }

            if (build)
                Build();

            if (deleteBuild && Directory.Exists(Settings.BuildPath))
                Directory.Delete(Settings.BuildPath, true);

            if (openBuildFolder)
                Process.Start("explorer.exe", Settings.ResourcesPath);
        }
    }
}
#endif