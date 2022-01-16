using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager: MonoBehaviour
{
    public static TooltipManager _instance;
    public TextMeshProUGUI textComponent;

    public void Initialise()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
        gameObject.SetActive(false);
    }
}