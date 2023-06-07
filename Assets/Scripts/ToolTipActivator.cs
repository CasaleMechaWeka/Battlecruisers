using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using UnityEngine.UI;

public class ToolTipActivator : MonoBehaviour
{


    public ToggleController toggleController;
    public GameObject toolTipTextDisplay;
    private bool started;

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            Image temp = gameObject.GetComponent<Image>();
            temp.enabled = toggleController.IsChecked.Value;
            toolTipTextDisplay.SetActive(toggleController.IsChecked.Value);
        }
    }

    public void Initialise()
    {
        started = true;
    }
}
