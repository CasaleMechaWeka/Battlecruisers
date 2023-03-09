using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;


namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {
        public override void OnPresenting(object activationParameter)
        {

        }


        public void Initialise(IScreensSceneGod screensSceneGod,
                           ISingleSoundPlayer soundPlayer,
                           IDataProvider dataProvider)
        {
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(dataProvider);
        }
    }
}

