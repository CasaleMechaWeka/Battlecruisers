using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Scenes.BattleScene
{
    public class CoinBattleHelper : NormalHelper
    {
        private readonly ICoinBattleModel _coinBattle;

        public CoinBattleHelper(
            IApplicationModel appModel,
            ILocTable storyStrings,
            PrefabFactory prefabFactory,
            IDeferrer deferrer)
            : base(appModel, storyStrings, prefabFactory, deferrer)
        {
            _coinBattle = DataProvider.GameModel.CoinBattle;
            Assert.IsNotNull(_coinBattle);
        }
    }
}
