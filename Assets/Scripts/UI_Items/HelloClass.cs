using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloClass : UI_Item {

	#region Params
	private TopPanelClass _topPnl;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		_topPnl = FindObjectOfType<TopPanelClass>();
	}

	protected override void OnEnable()
    {
		_topPnl.gameObject.SetActive(false);
        base.OnEnable();
    }

	protected override void OnDisable()
	{
		_topPnl.gameObject.SetActive(true);
		base.OnDisable();
	}

}
