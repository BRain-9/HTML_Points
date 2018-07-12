using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Saver : MonoBehaviour
{
	#region Params
	public const string POINTS_SAVE_TAG = "points_arr";
	public GameObject contentPointConteiner, contentPointPrefab;
	#endregion

	private IEnumerator Start()
	{
		yield return new WaitUntil(()=> WebRequester._loaded);
		ClearPointsContainer();
		LoadPoints();
	}

	private void OnDestroy()
	{
		
	}

	public void SavePoints()
	{
		Transform t = contentPointConteiner.transform;
		PointClass[] serializeItems = new PointClass[t.childCount];
		for (int i = 0; i < t.childCount; i++)
		{
			PointClass p = new PointClass();
			this.PrintLog(t.GetChild(i).gameObject.name);
			p.contentName = t.GetChild(i).gameObject.GetComponentInChildren<PowerUI.WorldUIHelper>().HtmlFile.name;
			p.position = t.GetChild(i).transform.position.ToString();
			serializeItems[i] = p;
		}
		string totalArr = JsonHelper.ToJson(serializeItems);
		Debug.Log(totalArr);
		PlayerPrefs.SetString(POINTS_SAVE_TAG, totalArr);
	}

	public void LoadPoints()
	{
		if (!PlayerPrefs.HasKey(POINTS_SAVE_TAG))
			return;
		string str = PlayerPrefs.GetString(POINTS_SAVE_TAG);
		Debug.Log(str);
		PointClass[] pointArr = JsonHelper.FromJson<PointClass>(str);
		for (int i = 0; i < pointArr.Length; i++)
		{
			Vector3 v = StringToVector3(pointArr[i].position);
			GameObject go = Instantiate(contentPointPrefab, v, Quaternion.identity
			                            ,contentPointConteiner.transform);
			go.SetActive(false);
            Destroy(go.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
			PowerUI.WorldUIHelper helper = go.GetComponentInChildren<PowerUI.WorldUIHelper>();
			helper.HtmlFile = ContentManager.GetHtml(pointArr[i].contentName); //Resources.Load(Constants.GameTags.contentFolder + "/" + pointArr[i].contentName) as TextAsset;
			go.SetActive(true);
		}
	}

	private void ClearPointsContainer()
	{
		if (PlayerPrefs.HasKey(POINTS_SAVE_TAG))
		{
			for (int i = 0; i < contentPointConteiner.transform.childCount; i++)
			{
				Destroy(contentPointConteiner.transform.GetChild(i).gameObject);
			}
		}
	}

	public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

}
