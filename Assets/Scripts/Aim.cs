using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour {

	#region Params
	private static Image _loader;
	private static bool load;

	public static float timePast;
	private static float clickTimer;
	private static bool clicked = false;

	private static GameObject clickWaiter;
	public delegate void AimDelegate (GameObject go);
	public static event AimDelegate clickedEvent = delegate{};

	#endregion

	private void Awake()
	{
		_loader = transform.GetChild(0).gameObject.GetComponent<Image>();
	}

    /// <summary>
    /// Starts the load.
    /// </summary>
    /// <param name="timer">Timer.</param>
	/// <param name="waiter">Object who wait click event.</param>
	public static void StartLoad (float timer, GameObject waiter)
	{
		if (timer > 0)
		{
			clickTimer = timer;
			clickWaiter = waiter;
			load = true;
		}
	}

	public static void StopLoad (GameObject invoker)
	{
		if (clickWaiter != null && invoker.Equals(clickWaiter))
		{
			load = false;
			_loader.fillAmount = 0f;
            clickWaiter = null;
			timePast = 0;
			clicked = false;
		}
	}

	public static float CustomLoad (float step)
	{
		if (_loader.fillAmount < 1)
		{
			_loader.fillAmount = step;
		}else {
			_loader.fillAmount = 0;
		}
		return _loader.fillAmount;
	}

	public static void ResetLoader()
	{
		_loader.fillAmount = 0;
	}

	void Update () {

		if (load && !clicked)
        {
            timePast += Time.deltaTime;

			if (timePast > clickTimer)
            {
                timePast = 0f;
				_loader.fillAmount = 0f;
                clicked = true;
				clickedEvent.Invoke(clickWaiter);
				StopLoad(clickWaiter);
            }

			_loader.fillAmount = timePast / clickTimer;
        }
	
	}
}
