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
    private ToolTipActivator toolTipActivator;
    private bool started = false;
    private int _virticalAdjustment = 0;
    private Color activeColor = new Color(0, 0, 0, 1);
    private Color notActiveColor = new Color(0, 0, 0, 0);

    public void Start()
    {
        textForLoc.enabled = false;
        toolTipText = ToolTipTextObject.GetComponentInChildren<Text>(includeInactive: true);
        background = ToolTipTextObject.GetComponent<Image>();
        toolTipText.enabled = false;
        background.enabled = false;
        toolTipActivator = ToolTipTextObject.GetComponent<ToolTipActivator>();
        started = true;
    }

    public void Update()
    {
        
    }

    private void adjustForHandheld() {
        _virticalAdjustment = -3;
        toolTipText.fontSize = 80;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (started && toolTipActivator.toggleController.IsChecked != null)
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                adjustForHandheld();
            }
            if (Input.mousePosition.x < Screen.width*3/4)
            {//left 3/4 of the screen
                ((RectTransform)ToolTipTextObject.transform).anchorMin = new Vector2(0, _virticalAdjustment);
                ((RectTransform)ToolTipTextObject.transform).anchorMax  = new Vector2(0, _virticalAdjustment);
                ((RectTransform)ToolTipTextObject.transform).pivot  = new Vector2(0, _virticalAdjustment);
            }
            else
            {//right 1/4 of the screen
                ((RectTransform)ToolTipTextObject.transform).anchorMin = new Vector2(1, _virticalAdjustment);
                ((RectTransform)ToolTipTextObject.transform).anchorMax  = new Vector2(1, _virticalAdjustment);
                ((RectTransform)ToolTipTextObject.transform).pivot  = new Vector2(1, _virticalAdjustment);
            }
            
            ToolTipTextObject.transform.position = Input.mousePosition;
            ToolTipTextObject.transform.position = new Vector3(ToolTipTextObject.transform.position.x, ToolTipTextObject.transform.position.y + 60, ToolTipTextObject.transform.position.z);
            if (!toolTipActivator.toggleController.IsChecked.Value)
            {
                hide();
            }

            toolTipText.text = textForLoc.text;
            show();
        }

    }

    private void show()
    {
        toolTipText.enabled = true;
        background.enabled = true;
        background.color = activeColor;
    }
    
    private void hide()
    {
        toolTipText.enabled = false;
        background.enabled = false;
        background.color = notActiveColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hide();
    }
}
