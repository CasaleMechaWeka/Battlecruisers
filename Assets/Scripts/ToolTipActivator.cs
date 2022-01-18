using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.UI.ScreensScene.SettingsScreen;

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
            toolTipTextDisplay.SetActive(toggleController.IsChecked.Value);
        }
    }

    public void Initialise()
    {
        started = true;
    }
}
