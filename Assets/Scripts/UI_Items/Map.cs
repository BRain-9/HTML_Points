using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PowerUI;
using System;



public class Map : UI_Item
{//, IPointerClickHandler {
	#region Params
	[SerializeField]
    GameObject contentPointPrefab, contentPointContainer;

	public float minPointDistance = 3f;

	private RaycastHit _hit;
	Vector3 tempPoint, finalPoint, menuPoint;
	public GameObject scrollView;
	private GameObject InvokedBtn;

	public Transform editMenuContent;
	Coroutine menuCoroutine;
	private bool _menuOpen,_loader,_editMode = true;
	public bool EditMode { get { return _editMode;} set { _editMode = value; }}
	private Saver _saver;
	private static ContentPoint _tempContent = null;
	public static ContentPoint ContentPoint 
	{
		get { return _tempContent; }
	}

	public Camera mapCamera;
	private MapImage _mapImg;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		bool ok = contentPointPrefab ?? true;
		Debug.Assert(ok, "Teleport point not inited!");
		_saver = FindObjectOfType<Saver>();
		_mapImg = GetComponentInChildren<MapImage>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		mapCamera.gameObject.SetActive(true);
		ContentBtn.contentBtnSystemEvent += (SystemBtnNames name) =>
		{
			switch (name)
			{
				case SystemBtnNames.Cancel:
					CloseEditMenu();
					this.PrintLog("Cancel");
					break;
				case SystemBtnNames.Delete:
					//if (InvokedBtn != null)
					Destroy(_tempContent.gameObject);    
					CloseEditMenu();
					this.PrintLog("Delete");
					break;
				default:
					break;
			}
		};
		ContentBtn.contentAddEvent += (string str, GameObject go) => { CloseEditMenu(); };
		Aim.clickedEvent += (GameObject GO) => {
			try
			{
				if (_tempContent == GO && _editMode)
                {
                    this.PrintLog("map Clicked!");
                    OpenEditMenu(GO);
                }
			}
			catch (System.Exception ex)
			{
				this.PrintLog(ex.Message, false, MsgType.Error);
			}
		};
		Player.Instance.mapMode = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		mapCamera.gameObject.SetActive(false);
		Player.Instance.mapMode = false;
		Aim.ResetLoader();
		CloseEditMenu();
		Close();
	}

	private void Update()
	{
		if (_editMode && !_menuOpen)
		{
			MyRay();
		}
	}

	private void MyRay()
	{
		var layerMask = (1 << LayerMask.NameToLayer("Roof"));
		layerMask |= Physics.IgnoreRaycastLayer;
        layerMask = ~layerMask;

		if (Physics.Raycast (MapImage.worldPoint,mapCamera.transform.forward*100f
		                     ,out _hit, Mathf.Infinity, layerMask) && _mapImg.IsOver)
		{
			this.PrintLog(_hit.collider.gameObject.name);
			if (_hit.collider.gameObject.GetComponent<ContentPoint>() != null)
			{
				if (!_loader)
				{
					_tempContent = _hit.collider.gameObject.GetComponent<ContentPoint>();
				}
			}else {
				if (!_loader)               
				    _tempContent = null;
			}
			tempPoint = _hit.point;
			if (menuCoroutine == null)
			{
				menuCoroutine = StartCoroutine(OpenMenu());
			}
		}
	}

	private IEnumerator OpenMenu ()
	{
		Vector3 temp = tempPoint;
		float minDistance = 0f;
		Transform t = contentPointContainer.transform;
		for (int i = 0; i < t.childCount; i++)
		{
			float tempF = Vector3.Distance(t.GetChild(i).transform.position
                                               , temp);
			if (i>0)
			{
                if (minDistance > tempF){
					minDistance = tempF;
				}
			}else {
				minDistance = tempF;
			}
		}
		yield return new WaitForSeconds(1f);
		if (!_menuOpen && DistanceCompare(minDistance,minPointDistance))
		{
			menuPoint = MapImage.mapPoint;
			this.PrintLog(menuPoint);
			this.PrintLog("SatrtWork");
			float f = 0f;
			while (f < 2f)//< 0.9)
			{
				f += Time.deltaTime;
				Aim.CustomLoad(f/2f);
				if (Vector3.Distance(temp, tempPoint) > 2f)
				{
					Aim.ResetLoader();
					menuCoroutine = null;
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			finalPoint = temp;
			OpenEditMenu();
		}
		menuCoroutine = null;
	}

	private bool DistanceCompare (float distance,float minDistance)
	{
		bool result = false;
		if (_tempContent == null)
		{
			result = distance == 0 ? true : distance > minDistance;
		}else 
		{
			result = true;
		}
		return result;
	}

	public void OpenEditMenu(GameObject btn = null)
	{
		Aim.ResetLoader();
		_loader = true;
        _menuOpen = true;
        InvokedBtn = btn;
		InteractToPoint();
	}

	private void ReplaceEditMenu(Vector3 point)
	{
		RectTransform rect = scrollView.GetComponent<RectTransform>();
		rect.anchoredPosition3D = new Vector3 (point.x,(point.y/2)- 60f, point.z);


	}

	private void InteractToPoint()
    {
        GameObject go;
        if (_tempContent == null)
            go = Instantiate(contentPointPrefab,
                                        new Vector3(finalPoint.x, 0
                                        , finalPoint.z), Quaternion.identity
                                        , contentPointContainer.transform);
        else
        {
            go = _tempContent.gameObject;
        }
        ContentPoint point = go.GetComponent<ContentPoint>();
        point.Selected(_tempContent == null ? Color.blue : Color.yellow);
		scrollView.GetComponent<ContentWindow>().destinationGO = go;
		ReplaceEditMenu(menuPoint);
		scrollView.SetActive(true);
    }

	public void CloseEditMenu()
	{
		_menuOpen = false;
		_loader = false;
		InvokedBtn = null;
        _tempContent = null;
		ContentPoint point;
		for (int i = 0; i < editMenuContent.childCount; i++)
        {
            Destroy(editMenuContent.GetChild(i).gameObject);
        }
		scrollView.SetActive(false);
	}

}
