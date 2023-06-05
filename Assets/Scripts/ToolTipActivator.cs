using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.ScreensScene.SettingsScreen;

public class ToolTipActivator : MonoBehaviour
{
    public PvPToggleController toggleController;
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
