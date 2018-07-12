using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using Utils;


public class ContentManager : MonoBehaviour
{
	#region Params
	private static string systemPath, contentFolderPath;
	#endregion

	private void Awake()
	{
		systemPath =
#if UNITY_ANDROID && !UNITY_EDITOR
			Application.persistentDataPath;
#elif UNITY_EDITOR
			Application.dataPath;
#endif
		contentFolderPath = Path.Combine(systemPath, Constants.GameTags.contentFolder);
		Debug.Assert(Directory.Exists(contentFolderPath), "Content folder is not created!");
	}

	public static TextAsset GetHtml(string fileName)
	{
		string filePath = Path.Combine(contentFolderPath, fileName + ".html");
		TextAsset result = null;
		if (File.Exists(filePath))
		{
			string str = string.Empty;
			str = File.ReadAllText(filePath);
			result = new TextAsset(str);
			result.name = fileName;
		}else {
			typeof (ContentManager).PrintLog("Incorrect path!" + filePath, false, MsgType.Error);
		}
		return result;
	}

	public static string [] GetContentNames ()
	{
		List<string> temp = new List<string>(Directory.GetFiles(contentFolderPath));
		string[] result;
		var v = from item in temp
                where !item.Contains(".html.meta")
                select item;
		foreach (var item in v)
		{
			Debug.Log(item);
		}
		temp = v.ToList();
		for (int i = 0; i < temp.Count; i++)
		{
			Debug.Log(temp[i]);
			temp[i] = Path.GetFileName(temp[i]).Replace(".html","");
		}
		result = temp.ToArray();
		return result;
	}
}
