using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentWindow : MonoBehaviour
{
	#region Params
	public GameObject btnPrefab, emptySpacePrefab;
	[SerializeField]
	private Transform btnParent;
	public Canvas canvas;
	[HideInInspector]
	public GameObject destinationGO;
	#endregion

	private void OnEnable()
	{
		CreateContentBtns();

		CreateSystemBtns();
	}

	private void CreateContentBtns()
	{
		var contentArr = ContentManager.GetContentNames(); //Resources.LoadAll(Constants.GameTags.contentFolder);
        for (int i = 0; i < contentArr.Length; i++)
        {
            GameObject temp = Instantiate(btnPrefab, btnParent);
            temp.SetActive(false);
            temp.GetComponent<UI_Button>().myCanvas = canvas;
			Debug.Log(contentArr[i]);
            temp.GetComponentInChildren<Text>().text = contentArr[i];
            temp.AddComponent<LayoutElement>();
			ContentBtn contentBtn = temp.AddComponent<ContentBtn>();
            contentBtn.destinationGO = this.destinationGO;
            temp.SetActive(true);
        }
	}

	private void CreateSystemBtns ()
	{
		string[] systemNames = System.Enum.GetNames(typeof(SystemBtnNames));
		for (int i = 0; i < systemNames.Length; i++)
		{
			if (Map.ContentPoint == null && systemNames[i] == SystemBtnNames.Delete.ToString())
			{
				continue;
			}
			if (systemNames[i] == SystemBtnNames.Delete.ToString())
			{
				Instantiate(emptySpacePrefab, btnParent);
			}
			GameObject systembtn = Instantiate(btnPrefab, btnParent);
			systembtn.SetActive(false);
			systembtn.GetComponentInChildren<Text>().text = systemNames[i];
			systembtn.AddComponent<LayoutElement>();
			ContentBtn contentBtn = systembtn.AddComponent<ContentBtn>();
			contentBtn.destinationGO = this.destinationGO;
			systembtn.GetComponent<UI_Button>().myCanvas = canvas;
			systembtn.SetActive(true);
		}
	}
}

public enum SystemBtnNames
{
	Cancel,
    Delete
}
