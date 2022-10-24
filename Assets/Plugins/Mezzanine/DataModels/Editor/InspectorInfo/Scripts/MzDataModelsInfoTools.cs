using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MzDataModelsInfoTools
{	
	static MzDataModelsInfoTools()
	{
		EditorApplication.delayCall += _SelectReadmeAutomatically;
	}
	
	private static void _SelectReadmeAutomatically()
	{
		if (SessionState.GetBool("MzDataModels.isShowedReadme", false)) return;
		
		// Re-serialize assets in the "Mezzanine/Common" folder, since we may have overwritten
		// a previous Mezzanine asset import.
		// See: https://answers.unity.com/questions/1625900/why-am-i-getting-assetimporter-is-referencing-an-a.html
		// And: https://garry.tv/unity-tips-2
		AssetDatabase.ForceReserializeAssets(new string[] { "Assets/Plugins/Mezzanine/Common" });
		
		_SelectReadme();
		SessionState.SetBool("MzDataModels.isShowedReadme", true);
	}
	
	[MenuItem("Tools/Mezzanine/Auto-Save")]
	private static MzInfo _SelectReadme()
	{
		var ids = AssetDatabase.FindAssets("MzDataModelsInfo t:MzInfo");

		if (ids.Length == 1)
		{
			var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
			Selection.objects = new UnityEngine.Object[] {readmeObject};
			return (MzInfo)readmeObject;
		}

		Debug.Log("Couldn't find a readme");
		return null;
	}
}
