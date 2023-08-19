using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class SelectHeckleButton : CanvasGroupButton
    {
        public TextMeshProUGUI limit;
        public GameObject selectText;
        public GameObject deselectText;
        public int heckleLimit;
        public GameObject checkBox;
    }
}
