using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Scenes.BattleScene
{
    public class CoinBattleHelper : NormalHelper
    {
        private readonly ICoinBattleModel _coinBattle;

        public CoinBattleHelper(
            IApplicationModel appModel,
            IPrefabFetcher prefabFetcher,
            ILocTable storyStrings,
            IPrefabFactory prefabFactory,
            IDeferrer deferrer)
            : base(appModel, prefabFetcher, storyStrings, prefabFactory, deferrer)
        {
            _coinBattle = DataProvider.GameModel.CoinBattle;
            Assert.IsNotNull(_coinBattle);
        }
        public override ILevel GetLevel()
        {
            int maxLevel = DataProvider.GameModel.NumOfLevelsCompleted - 1; //might need null or not-0 check?
            int levelIndex = UnityEngine.Random.Range(1, maxLevel);
            return _appModel.DataProvider.GetLevel(levelIndex);
        }
    }
}
