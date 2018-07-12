using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.UI;
using PowerUI;


public class ContentPoint : VRInteractiveItem
{
	#region Params
	public static event System.Action<Vector3, GameObject> contentPointEvent = delegate { };
	private Renderer _myColor;
	[SerializeField]
	private WorldUIHelper html_Helper;
	private Saver _saver;
	private Player player;
	private float watchToPlayerDistance = 10f;
	#endregion

	private void Awake()
	{
		Init();
	}

	private void OnEnable()
	{
		ContentBtn.contentAddEvent += OnContetnBtnEvent;
		ContentBtn.contentBtnSystemEvent += MenuClosed;
	}

	private void OnDestroy()
	{
		ContentBtn.contentAddEvent -= OnContetnBtnEvent;
		ContentBtn.contentBtnSystemEvent -= MenuClosed;
	}

	private void OnContetnBtnEvent(string fileName, GameObject go)
	{
		if (go.Equals(this.gameObject))
		{
			this.PrintLog("Path is" + fileName);
			ChangeContent(fileName);
		}
	}

	private void Update()
	{
		LookToPlayer();
	}

	private void LookToPlayer()
	{
		bool look = Vector3.Distance(player.gameObject.transform.position
                                                           , this.gameObject.transform.position)
                                                            < watchToPlayerDistance;
		if (look)
		{
			this.PrintLog("Look");
			transform.LookAt (player.transform);
		}
	}

	private void Init()
	{
		bool ok = true;
		_myColor = this.gameObject.GetComponent<Renderer>();
		ok = html_Helper ?? true;
		Debug.Assert(ok, "Html helper need!");
		_saver = FindObjectOfType<Saver>();
		player = FindObjectOfType<Player>();
	}

	private void  MenuClosed(SystemBtnNames name)
	{
		if (name == SystemBtnNames.Cancel)
		{
			if (html_Helper.HtmlFile == null)
			{
				Destroy(this.gameObject);
			}
			else
			{
				Selected(Color.white);
			}
		}
	}

	public void ChangeContent(string path)
	{
		bool ok = !string.IsNullOrEmpty(path);
		Debug.Assert(ok,"Path is null");
		html_Helper.gameObject.SetActive  (false);
		html_Helper.HtmlFile = ContentManager.GetHtml(path);//Resources.Load(path) as TextAsset;
		for (int i = 0; i < html_Helper.gameObject.transform.childCount; i++)
		{
			Destroy(html_Helper.gameObject.transform.GetChild(i).gameObject);
		}
		html_Helper.gameObject.SetActive (true);
		Selected(Color.white);
		_saver.SavePoints();
	}

	public void Selected(Color color)
	{
		this.PrintLog("Color must be" + color.ToString());
		_myColor.material.color = new Color (color.r,color.g,color.b,color.a);
	}
}
