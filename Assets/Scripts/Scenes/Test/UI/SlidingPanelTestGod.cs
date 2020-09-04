using BattleCruisers.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    public class SlidingPanelTestGod : MonoBehaviour
    {
        public SlidingPanel panel;

        void Start()
        {
            Assert.IsNotNull(panel);
            panel.Initialise();
        }
    }
}