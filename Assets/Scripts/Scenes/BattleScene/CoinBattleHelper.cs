using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class CoinBattleHelper : NormalHelper
    {
        private readonly CoinBattleModel _coinBattle;

        public CoinBattleHelper(IDeferrer deferrer)
            : base(deferrer)
        {
            _coinBattle = DataProvider.GameModel.CoinBattle;
            Assert.IsNotNull(_coinBattle);
        }
    }
}
