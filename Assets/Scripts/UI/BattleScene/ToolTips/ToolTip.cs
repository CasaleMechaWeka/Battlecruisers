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
    private bool exited = true;

    public void Start()
    {
        textForLoc.enabled = false;
        toolTipText = ToolTipTextObject.GetComponentInChildren<Text>(includeInactive: true);
        background = ToolTipTextObject.GetComponent<Image>();
        toolTipText.enabled = false;
        background.enabled = false;
    }

    public void Update()
    {
        
        if (Input.mousePosition.x < Screen.width*3/4)
        {
            xDelta = (((RectTransform)ToolTipTextObject.transform).rect.width)/3;
        }
        else{
            xDelta = -(((RectTransform)ToolTipTextObject.transform).rect.width)/3;
        }
        
        ToolTipTextObject.transform.position = Input.mousePosition;
        ToolTipTextObject.transform.position = new Vector3(ToolTipTextObject.transform.position.x + xDelta, ToolTipTextObject.transform.position.y + 60, ToolTipTextObject.transform.position.z);
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipText.text = textForLoc.text;
        show();
    }

    private void show()
    {
        toolTipText.enabled = true;
        background.enabled = true;
    }
    
    private void hide()
    {
        toolTipText.enabled = false;
        background.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hide();
    }
}
