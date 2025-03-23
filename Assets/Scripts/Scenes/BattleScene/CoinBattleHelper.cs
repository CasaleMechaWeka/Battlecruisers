using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class CoinBattleHelper : NormalHelper
    {
        private readonly ICoinBattleModel _coinBattle;

        public CoinBattleHelper(
            IApplicationModel appModel,
            PrefabFactory prefabFactory,
            IDeferrer deferrer)
            : base(appModel, prefabFactory, deferrer)
        {
            _coinBattle = DataProvider.GameModel.CoinBattle;
            Assert.IsNotNull(_coinBattle);
        }
    }
}
