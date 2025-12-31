using BattleCruisers.UI.ScreensScene.SettingsScreen;
using UnityEngine;

public class PvPToolTipActivator : MonoBehaviour
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
