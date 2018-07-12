using UnityEditor;
using UnityEngine;
public class HotKeys : EditorWindow
{
	#region Params

    #endregion

	[MenuItem ("HotKeys/Switch _`")]
	private static void Switch ()
	{
		var gos = Selection.gameObjects;
		for (int i = 0; i < gos.Length; i++)
		{
			gos[i].SetActive(!gos[i].activeInHierarchy);
		}
	}

	[MenuItem ("HotKeys/ClearPrefs")]
	private static void Clear()
	{
		PlayerPrefs.DeleteAll();
	}
}

