using BattleCruisers.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    // FELIX Remove? :P
    public class SlidingPanelTestGod : MonoBehaviour
    {
        public SlidingPanel panel;

        void Start()
        {
            Assert.IsNotNull(panel);
        }

        public void Show()
        {
            panel.Show();
        }

        public void Hide()
        {
            panel.Hide();
        }
    }
}