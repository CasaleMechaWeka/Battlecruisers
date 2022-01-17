using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager _instance;
    public Text textComponent;

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    public void Start()
    {
        textComponent.text = "";
        gameObject.SetActive(false);
    }

    //may have issues with touch versus mouse
    public void SetAndShowToolTip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
        transform.position = Input.mousePosition;
        if (transform.position.x < Screen.width/2)
        {
            textComponent.alignment = TextAnchor.LowerRight;
        }
        else{
            textComponent.alignment = TextAnchor.LowerLeft;
        }
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = "";
    }
    
}
