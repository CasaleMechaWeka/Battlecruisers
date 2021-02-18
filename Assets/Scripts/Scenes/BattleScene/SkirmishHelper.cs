using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        private readonly IRandomGenerator _random;
        private readonly StrategyType _strategyType;

        public SkirmishHelper(
            IApplicationModel appModel,
            IPrefabFetcher prefabFetcher,
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer,
            StrategyType strategyType) 
            : base(appModel, prefabFetcher, prefabFactory, deferrer)
        {
            _random = RandomGenerator.Instance;
            _strategyType = strategyType;
        }

        public override ILevel GetLevel()
        {
            // FELIX  Make level background have same sky as in campaign?  Check with Pete :)
            int levelNum = -99;  // Unused for skirmish
            IPrefabKey hull = _random.RandomItem(_appModel.DataProvider.StaticData.HullKeys);
            SoundKeyPair musicKeys = _random.RandomItem(SoundKeys.Music.Background.All);
            string skyMaterialName = _random.RandomItem(SkyMaterials.All);

            return
                new Level(
                    levelNum,
                    hull,
                    musicKeys,
                    skyMaterialName);
        }

        protected override IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            return new SkirmishStrategyFactory(_strategyType);
        }

        public override Task<string> GetEnemyNameAsync(int levelNum)
        {
            // FELIX  Do we want an enemy name?
            return Task.FromResult("SKIRMISH");
        }

        public override Task<IPrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            // FELIX :D
            throw new NotImplementedException();
        }
    }
}