using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : UI_Item {

	#region Params
	public Canvas myCanvas;
	#endregion

	protected override void Awake()
	{
		//base.Awake();
	}

	protected override void OnEnable()
	{
		base._parentCanvas = myCanvas;
		base.OnEnable();
	}

	public override void Selected()
	{
		base.Selected();
		UI_Item.bntOver = true;
	}
	public override void Deselected()
	{
		//base.Deselected();
		UI_Item.bntOver = false;
	}
}
