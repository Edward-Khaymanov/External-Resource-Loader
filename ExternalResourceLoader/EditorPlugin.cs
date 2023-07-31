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
        private const string LOCAL_BUILD_PATH_VARIABLE = "Local.BuildPath";
        private const string LOCAL_LOAD_PATH_VARIABLE = "Local.LoadPath";
        private const string MONOSCRIPT_BUNDLE_CUSTOM_NAMING = "_ExternalResource_";

        [MenuItem("Window/External Resource Loader/Show")]
        public static void ShowWindow()
        {
            GetWindow<EditorPlugin>("External Resource Loader");
        }

        [MenuItem("Window/External Resource Loader/Init")]
        public static void Init()
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
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

            Directory.CreateDirectory(Settings.ResourcesPath);
        }

        private void OnGUI()
        {
            var buttonSettings = GUILayout.Height(20);
            var buildOverwrite = GUILayout.Toggle(true, "Overwrite if the build folder exists");
            var build = GUILayout.Button("Build", buttonSettings);
            var deleteBuild = GUILayout.Button("Delete build", buttonSettings);
            var openResourcesFolder = GUILayout.Button("Open resources folder", buttonSettings);
            var copyDll = GUILayout.Button("Copy dll to resources folder", buttonSettings);
            if (copyDll)
                CopyDll();

            if (build)
                Build(buildOverwrite);

            if (deleteBuild)
                DeleteBuild();

            if (openResourcesFolder)
                Process.Start("explorer.exe", Settings.ResourcesPath);
        }

        private void Build(bool overwrite)
        {
            if (overwrite)
                DeleteBuild();

            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
        }

        private void DeleteBuild()
        {
            if (Directory.Exists(Settings.BuildPath))
                Directory.Delete(Settings.BuildPath, true);
        }

        private void CopyDll()
        {
            var projectFolder = Directory.GetParent(Application.dataPath);
            var selectedDllPath = EditorUtility.OpenFilePanelWithFilters("Select dll", projectFolder.FullName, new[] { "Dll files", "dll" });

            if (string.IsNullOrEmpty(selectedDllPath))
                return;

            var file = new FileInfo(selectedDllPath);
            if (file.Exists == false)
                return;

            var targetPath = Path.Combine(Settings.ResourcesPath, file.Name);
            file.CopyTo(targetPath, true);
        }
    }
}
#endif