using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostTutorialButtonsPanel : ButtonsPanel
    {
        public override void Initialise(IPostBattleScreen postBattleScreen, ISoundPlayer soundPlayer)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            ActionButton retryButton = transform.FindNamedComponent<ActionButton>("SmallButtons/RetryButton");
            retryButton.Initialise(postBattleScreen.RetryTutorial);

            ActionButton nextButton = transform.FindNamedComponent<ActionButton>("NextButton");
            nextButton.Initialise(postBattleScreen.StartLevel1);
        }
    }
}