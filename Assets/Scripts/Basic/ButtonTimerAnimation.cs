using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonTimerAnimation : MonoBehaviour  
{

    public Button button;
    public Image fill;
    public float value;
    public float timePast = 0f;
    public float clickTimer = 2f;

    public bool over = false;
    public VRInteractiveItem item;

    private bool clicked = false;

    private void OnEnable()
    {
        item = GetComponent<VRInteractiveItem>();

        if (item == null)
        {
            Debug.Log("VRInteractiveItem not found at " + gameObject.name);
        }
        else
        {
            item.OnOver += HandleOver;
            item.OnOut += HandleOut;

            item.OnDown += HandleDown;
            item.OnUp += HandleUp;
        }


    }


    private void OnDisable()
    {
        if (item != null)
        {
            item.OnDown -= HandleDown;
            item.OnUp -= HandleUp;

            item.OnOver -= HandleOver;
            item.OnOut -= HandleOut;
        }
    }

    void Start () {

        button = gameObject.GetComponent<Button>();
        fill = button.transform.GetChild(0).GetComponent<Image>();
      
	}



    // Update is called once per frame
    void Update()
    {

        if (over && !clicked)
        {
            timePast += Time.deltaTime;

            if (timePast >= clickTimer)
            {
                timePast = 0f;
                fill.fillAmount = 0;
                over = false;

                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerClickHandler);
                clicked = true;
            }

            fill.fillAmount = timePast / clickTimer;
        }

    }

    public void HandleOver()
    {
        over = true;
    }

    public void HandleOut()
    {
        over = false;

        timePast = 0f;
        fill.fillAmount = 0f;
        clicked = false;
    }

    public void HandleDown()
    {
        
    }

    public void HandleUp()
    {

    }
}
