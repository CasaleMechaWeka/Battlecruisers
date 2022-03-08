using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToolTipTextObject;
    private Text toolTipText;
    private Image background;
    public Text textForLoc;
    private float xDelta;

    public void Start()
    {
        textForLoc.enabled = false;
        toolTipText = ToolTipTextObject.GetComponentInChildren<Text>();
        background = ToolTipTextObject.GetComponent<Image>();
        toolTipText.enabled = false;
        background.enabled = false;
    }

    public void Update()
    {
        if (toolTipText.enabled)
        {
            if (Input.mousePosition.x < Screen.width/2)
            {
                xDelta = (((RectTransform)ToolTipTextObject.transform).rect.width)/3;
            }
            else{
                xDelta = -(((RectTransform)ToolTipTextObject.transform).rect.width)/3;
            }
            if (Mathf.Abs(xDelta) > 10)
            {
                ToolTipTextObject.transform.position = Input.mousePosition;
                ToolTipTextObject.transform.position = new Vector3(ToolTipTextObject.transform.position.x + xDelta, ToolTipTextObject.transform.position.y + 60, ToolTipTextObject.transform.position.z);
            } 
        }
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipText.enabled = false;
        background.enabled = false;
        toolTipText.text = "";
        if (Input.mousePosition.x < Screen.width/2)
        {
            xDelta = (((RectTransform)ToolTipTextObject.transform).rect.width)/3;
        }
        else{
            xDelta = -(((RectTransform)ToolTipTextObject.transform).rect.width)/3;
        }
        if (Mathf.Abs(xDelta) > 10)
        {
            ToolTipTextObject.transform.position = Input.mousePosition;
            ToolTipTextObject.transform.position = new Vector3(ToolTipTextObject.transform.position.x + xDelta, ToolTipTextObject.transform.position.y + 60, ToolTipTextObject.transform.position.z);
        }
        toolTipText.text = textForLoc.text;
        toolTipText.enabled = true;
        background.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipText.enabled = false;
        background.enabled = false;
        toolTipText.text = "";
    }
}
