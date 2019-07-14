using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleButtonsPanel : ButtonsPanel
    {
        public void Initialise(IPostBattleScreen postBattleScreen, ICommand nextCommand)
        {
            base.Initialise(postBattleScreen);

            Assert.IsNotNull(nextCommand);

            ActionButton loadoutButton = transform.FindNamedComponent<ActionButton>("SmallButtons/LoadoutButton");
            loadoutButton.Initialise(postBattleScreen.GoToLoadoutScreen);

            ButtonController nextButton = GetComponentInChildren<ButtonController>();
            Assert.IsNotNull(nextButton);
            nextButton.Initialise(nextCommand);
        }
    }
}