using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelClass : UI_Item
{
	#region Params
	[SerializeField]
	private GameObject map, contentPointContainer;
	private ContentPoint _contentPoint;
	[SerializeField]
	Button openMapBtn, closeMapBtn, editModeBtn, clearBtn;
    /// <summary>
    /// The player position and rotation before change position to map mode.
    /// </summary>
	private Vector3 playerPos, playerRot;
	public Image EditCheckBox;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		bool ok = map ?? true;
		Debug.Assert(ok, "Map window not initialized!");
	}

	public void OpenMap()
	{
		closeMapBtn.gameObject.SetActive(true);
		editModeBtn.gameObject.SetActive(true);
		clearBtn.gameObject.SetActive(true);
		openMapBtn.gameObject.SetActive(false);
		map.SetActive(true);
		transform.parent.gameObject.GetComponentInParent<CameraFollow>().ry = true;
	}
    
	public void CloseMap()
	{
		closeMapBtn.gameObject.SetActive(false);
		editModeBtn.gameObject.SetActive(false);
		clearBtn.gameObject.SetActive(false);
		openMapBtn.gameObject.SetActive(true);
		map.SetActive(false);
		transform.parent.gameObject.GetComponentInParent<CameraFollow>().ry = false;
	}

	public void EditorModeSwitcher ()
	{
		EditCheckBox.enabled = map.GetComponent<Map>().EditMode = !map.GetComponent<Map>().EditMode;
	}

	public void ClearHtmlPoints ()
	{
		if (PlayerPrefs.HasKey(Saver.POINTS_SAVE_TAG))
        {
            for (int i = 0; i < contentPointContainer.transform.childCount; i++)
            {
				Destroy(contentPointContainer.transform.GetChild(i).gameObject);
            }
        }
		PlayerPrefs.DeleteKey(Saver.POINTS_SAVE_TAG);
	}
}
