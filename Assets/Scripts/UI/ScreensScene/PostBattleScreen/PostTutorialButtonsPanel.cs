using BattleCruisers.Data.Models;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostTutorialButtonsPanel : ButtonsPanel
    {
        public void Initialise(IPostBattleScreen postBattleScreen, ISingleSoundPlayer soundPlayer, IGameModel gameModel)
        {
            base.Initialise(postBattleScreen, soundPlayer);

            Assert.IsNotNull(gameModel);

            CanvasGroupButton nextButton = transform.FindNamedComponent<CanvasGroupButton>("NextButton");

            if (gameModel.FirstNonTutorialBattle)
            {
                nextButton.Initialise(soundPlayer, postBattleScreen.GoToChooseDifficultyScreen);
                Destroy(homeButton.gameObject);
            }
            else
            {
                nextButton.Initialise(soundPlayer, postBattleScreen.StartLevel1);
            }

            gameObject.SetActive(true);
        }
    }
}