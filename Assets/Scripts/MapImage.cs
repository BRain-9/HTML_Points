using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MapImage : UI_Item, IPointerClickHandler
{
#region Params
	public RectTransform rect;
	public Transform camGlob;
	public Camera camMap;
	private Vector3 posCamera;
	private RaycastHit _hit;
	public static Vector3 worldPoint, mapPoint;
	public Canvas canvas;
	#endregion

	protected override void Awake()
	{
		//base.Awake(); 
		_parentCanvas = canvas;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Deselected();
		Selected();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Vector2 vec;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
			eventData.pointerCurrentRaycast.screenPosition,
			eventData.enterEventCamera, out vec);
		float x = (camMap.pixelWidth * rect.pivot.x) + (camMap.pixelWidth * (vec.x / rect.sizeDelta.x));

		float y = camMap.pixelHeight * rect.pivot.y + camMap.pixelHeight * (vec.y / rect.sizeDelta.y);

		var pos = new Vector3(x, y, camMap.transform.position.y);

		posCamera = camMap.ScreenToWorldPoint(pos);

		//camGlob.transform.position = new Vector3(posCamera.x, camGlob.transform.position.y,
		//posCamera.z);
	}

	private void Update()
	{
		//Debug.DrawRay(new Vector3 (posCamera.x,camMap.transform.position.y,posCamera.z)
		//,camMap.transform.forward * 100f, Color.black);
		worldPoint =  Cast();
	}

	public Vector3 Cast()
	{
		var layerMask = (1 << LayerMask.NameToLayer("UI"));
        layerMask |= Physics.IgnoreRaycastLayer;
        layerMask = ~layerMask;
		Vector3 result = Vector3.zero;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 100f, out _hit))
		{
			if (_hit.collider.gameObject.Equals(this.gameObject))
			{
				Vector3 vector = _hit.collider.gameObject.
									 transform.InverseTransformPoint(_hit.point);
				mapPoint = new Vector3 (vector.x,vector.y, vector.z);
				RectTransform rect = _hit.collider.gameObject.GetComponent<RectTransform>();
				float x = vector.x / rect.rect.width;
				float y = vector.y / rect.rect.height;
				Vector2 temp = new Vector2(x, y);
				Vector3 camPos = new Vector3((camMap.orthographicSize * 2) * temp.x
											 , (camMap.orthographicSize * 2) * temp.y, 0f);
				var v = camMap.transform.TransformPoint(camPos);
				Debug.DrawRay(v, camMap.transform.forward * 100f, Color.red);
				result = v;
			}
		}
		return result;
	}
}

