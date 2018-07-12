using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	#region Params 
	private static Player _instance;
	public static Player Instance{ get { return _instance; }}
	[HideInInspector]
	public bool mapMode;
	[SerializeField]
	private Map _map;
	#endregion

	private void Awake()
	{
		if (_instance != null && _instance != this){
			Destroy(this);
		}
		_instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	private void OnEnable()
	{
		ContentPoint.contentPointEvent += TeleportPointMeth;
	}

	private void OnDisable()
	{
		ContentPoint.contentPointEvent -= TeleportPointMeth;
	}

	private void Update()
	{
		QuitApp();
	}

	private void TeleportPointMeth (Vector3 pos, GameObject btn)
	{
		if (!mapMode)
		{
			this.transform.position = pos;
		}else {
			_map.OpenEditMenu(btn);
		}
	}

	private void QuitApp ()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
