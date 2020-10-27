using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostTutorialButtonsPanel : ButtonsPanel
    {
        public override void Initialise(IPostBattleScreen postBattleScreen, ISingleSoundPlayer soundPlayer)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            CanvasGroupButton nextButton = transform.FindNamedComponent<CanvasGroupButton>("NextButton");
            nextButton.Initialise(soundPlayer, postBattleScreen.StartLevel1);
        }
    }
}