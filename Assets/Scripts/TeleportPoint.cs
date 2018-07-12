using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class TeleportPoint : VRInteractiveItem{

	#region Params
	private Player _player;
	private GameObject startScreen;
	#endregion

	private void Awake()
	{
		_player = FindObjectOfType<Player>();
		startScreen = FindObjectOfType<HelloClass>().gameObject;
	}

	private void OnEnable()
	{
		OnOver += () => {
			if (!startScreen.activeInHierarchy)
			    Aim.StartLoad(2f, this.gameObject);
        };
        Aim.clickedEvent += this.OnClick;
        OnOut += () => {
			if (!startScreen.activeInHierarchy)
                Aim.StopLoad(this.gameObject);
        };
	}

	new private void OnClick(GameObject go)
    {
		if (this.gameObject == go)
		{
			_player.gameObject.transform.position = this.gameObject.transform.position;	
		}
    }
}
