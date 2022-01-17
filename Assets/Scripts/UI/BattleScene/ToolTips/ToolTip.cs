using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ToolTipTextObject;
    private Text toolTipText;
    public Text textForLoc;

    public void Start()
    {
        textForLoc.enabled = false;
        toolTipText = ToolTipTextObject.GetComponent<Text>();
        toolTipText.enabled = false;
    }

    public void Update()
    {
        if (toolTipText.enabled)
        {
            ToolTipTextObject.transform.position = Input.mousePosition;
            ToolTipTextObject.transform.position = new Vector3(ToolTipTextObject.transform.position.x, ToolTipTextObject.transform.position.y + 60, ToolTipTextObject.transform.position.z);
            
        }
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipText.text = textForLoc.text;
        toolTipText.enabled = true;
        ToolTipTextObject.transform.position = Input.mousePosition;
        ToolTipTextObject.transform.position = new Vector3(ToolTipTextObject.transform.position.x, ToolTipTextObject.transform.position.y + 60, ToolTipTextObject.transform.position.z);
        if (ToolTipTextObject.transform.position.x < Screen.width/2)
        {
            toolTipText.alignment = TextAnchor.LowerLeft;
        }
        else{
            toolTipText.alignment = TextAnchor.LowerRight;
        }
        Debug.Log("WOW");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipText.enabled = false;
        toolTipText.text = "";
        Debug.Log("WOW2");
    }
}
