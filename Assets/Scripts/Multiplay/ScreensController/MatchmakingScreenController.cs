using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;


namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {

   
        public override void OnPresenting(object activationParameter)
        {

        }


        public void Initialise(IMultiplayScreensSceneGod multiplayScreensSceneGod,
                           ISingleSoundPlayer soundPlayer,
                           IDataProvider dataProvider)
        {
            base.Initialise(multiplayScreensSceneGod);
            Helper.AssertIsNotNull(dataProvider);
        }
    }
}

