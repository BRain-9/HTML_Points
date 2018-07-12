using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentBtn : MonoBehaviour {

	#region Params
	Button _myBtn;
    
	/// <summary>
    /// Start changing content. Param1 for content name, param2 for destination game object.
    /// </summary>
	public static event System.Action <string, GameObject> contentAddEvent = delegate {};
    
	public static event System.Action<SystemBtnNames> contentBtnSystemEvent = delegate {};

	Text _myNameText;

	public GameObject destinationGO;
    #endregion

	private void OnEnable()
	{
		Init();
	}
	private void Update()
	{
		ResizeCollider();
	}
	private void OnClick ()
	{
		Debug.Log(_myNameText.text);
        switch (_myNameText.text)
        {
			case "Cancel":
				contentBtnSystemEvent.Invoke(SystemBtnNames.Cancel);
                break;
            case "Delete":
				contentBtnSystemEvent.Invoke(SystemBtnNames.Delete);
				break;
            default:
				contentAddEvent.Invoke(_myNameText.text, destinationGO);
                break;
        }
	}

	private void ResizeCollider()
	{
		this.gameObject.GetComponent<BoxCollider>().size = new Vector2(this.GetComponent<RectTransform>().rect.width
		                                                               ,this.GetComponent<RectTransform>().rect.height);
		this.gameObject.GetComponent<BoxCollider>().center = new Vector3(this.GetComponent<RectTransform>().rect.width/2
                                                                       , this.GetComponent<RectTransform>().rect.height/-2,-1); 
	}

	private void Init()
	{
		_myBtn = GetComponent<Button>();
        _myNameText = GetComponentInChildren<Text>();
		_myNameText.resizeTextForBestFit = true;
		_myBtn.onClick.AddListener(OnClick);
		ResizeCollider();
	}
}
