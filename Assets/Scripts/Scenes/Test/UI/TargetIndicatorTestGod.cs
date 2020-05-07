using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene;
using UnityEngine;
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
        }

        public void ShowTargetIndicator(Vector2 position)
        {

        }

        public void HideTargetIndicator()
        {
            targetIndicator.Hide();
        }
    }
}