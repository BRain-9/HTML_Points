using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class UI_Item : VRInteractiveItem, IUI_Item {
	#region Params
	public static bool bntOver;
	protected Canvas _parentCanvas;
    #endregion

	protected virtual void Awake()
    {
        _parentCanvas = transform.parent.GetComponent<Canvas>();
		bool ok = GetComponent<Collider>() ?? true;
		Debug.Assert(ok, this.gameObject.name + "need collider");
    }

	protected virtual void OnEnable()
	{
		OnOver += Selected;
		OnOut += Deselected;
	}
	protected virtual void OnDisable()
    {
		OnOver -= Selected;
		OnOut -= Deselected;
    }

	public virtual void OnOpen()
    {
       
    }

	public virtual void Close()
    {
		//_parentCanvas.GetComponentInParent<CameraFollow>().rx = true;
        try
		{
			_parentCanvas.gameObject.GetComponentInParent<CameraFollow>().ry = true;
            
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
		this.gameObject.SetActive(false);
    }

	public virtual void Selected ()
	{
		_parentCanvas.gameObject.GetComponentInParent<CameraFollow>().rx = false;
		_parentCanvas.gameObject.GetComponentInParent<CameraFollow>().ry = false;
		Debug.Log("Over" + bntOver);
	}

	public virtual void Deselected()
	{
			//_parentCanvas.GetComponentInParent<CameraFollow>().rx = true;
			//_parentCanvas.GetComponentInParent<CameraFollow>().ry = true;
		StartCoroutine(Deselect());
	}

	private IEnumerator Deselect()
	{
		Debug.Log ("Deselcting. Over is " + bntOver);
		yield return new WaitForEndOfFrame();
		Debug.Log("Deselcting. Over is " + bntOver);
		if (!bntOver)
		{
			_parentCanvas.GetComponentInParent<CameraFollow>().ry = true;
		}
	}

}
