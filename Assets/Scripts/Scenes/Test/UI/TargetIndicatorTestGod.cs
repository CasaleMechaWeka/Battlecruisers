using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    public class TargetIndicatorTestGod : TestGodBase
    {
        public TargetIndicatorController targetIndicator;

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(targetIndicator);
            targetIndicator.Initialise();

            TargetButton[] buttons = FindObjectsOfType<TargetButton>();
            foreach (TargetButton button in buttons)
            {
                button.Initialise(targetIndicator);
            }
        }

        public void HideTargetIndicator()
        {
            targetIndicator.Hide();
        }
    }
}