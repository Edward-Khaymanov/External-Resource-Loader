# External Resource Loader
External Resource Loader is a tool that helps you configure UNITY to load mods/resources from a local disk using unity addresables


## Requirements
- Addresables

## Setup
1. Import package in your project
2. Click `Window/External Resource Loader/Init` - this will set up your addressables settings, create a new profile and create ExternalResources folder

## Plugin overview
Click `Window/ExternalResourceLoader/Show` - this will open the plugin window

- Build - creates a folder at `Settings.BuildPath` in which there will be a folder `Data` in which there will be resources marked as Addresable
- Delete build - delete folder at `Settings.BuildPath`
- Open resource folder - open folder with external assets
- Copy dll - select you scripts dll (by default it is Assembly-CSharp) and copy it to folder with external assets (`Settings.ResourcesPath`)

## Usage
### Export
1. Make you gameobject addresable
2. Set label for this object
3. Click build in plugin window

If you have custom scripts in your assets, copy the assembly where your script is stored (by default it is `Assembly-CSharp`). <br/>
> **Warning** <br/>
> Make sure that for the release version of the game you copy the dll from the folder with the release build, and for the development build or unity editor you can use it from the folder `pathToYouProject\Library\ScriptAssemblies`

### Import

Using the methods below, you will be able to get the object by specifying the path to the location of the resources and the label of the object

```C#

using System.IO;
using ExternalResourceLoader;

public class Test
{
	public GameObject SomeGameObjectMethod(string pathToExternalResource, string gameObjectLabel)
	{
		var assetLoader = new ExternalAssetLoader();
		var externalResourceDataPath = Path.Combine(pathToExternalResource, ExternalResourceLoader.Settings.DataFolderName);
		return assetLoader.GetGameObject(externalResourceDataPath, gameObjectLabel);
	}
	
	
	public SomeScriptableObject SomeScriptableObjectMethod(string pathToExternalResource, string scriptableObjectLabel)
	{
		var assetLoader = new ExternalAssetLoader();
		var externalResourceDataPath = Path.Combine(pathToExternalResource, ExternalResourceLoader.Settings.DataFolderName);
		return assetLoader.GetScriptableObject(externalResourceDataPath, scriptableObjectLabel) as SomeScriptableObject;
	}
}

```

If you have custom scripts in your assets, you need to load the assembly where your script is stored (by default it is Assembly-CSharp) using 
```C#
new DllLoader().Load(pathToDll)
```
> **Warning** <br/>
> To unload your build, you need to unload all the application domains that contain it. Unity does not support multiple domains and because of this, you will not be able to unload the assembly. You have to reload game to avoid memory clogging.
















