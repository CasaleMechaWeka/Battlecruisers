using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : ITutorialArgs
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public INavigationButtonsWrapper NavigationButtonsWrapper { get; private set; }
        public IGameSpeedWrapper GameSpeedWrapper { get; private set; }

        public TutorialArgs(ICruiser playerCruiser, ICruiser aiCruiser, IHUDCanvasController hudCanvas)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, hudCanvas);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            NavigationButtonsWrapper = hudCanvas.NavigationButtonsWrapper;
            GameSpeedWrapper = hudCanvas.GameSpeedWrapper;
        }
    }
}
