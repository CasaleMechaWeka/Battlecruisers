using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostTutorialButtonsPanel : ButtonsPanel
    {
        public override void Initialise(IPostBattleScreen postBattleScreen)
        {
            base.Initialise(postBattleScreen);

            ActionButton nextButton = transform.FindNamedComponent<ActionButton>("NextButton");
            nextButton.Initialise(postBattleScreen.StartLevel1);
        }
    }
}