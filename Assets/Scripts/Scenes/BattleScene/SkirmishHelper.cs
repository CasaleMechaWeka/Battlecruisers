using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        private readonly IRandomGenerator _random;
        private readonly ISkirmishModel _skirmish;

        public SkirmishHelper(
            IApplicationModel appModel,
            IPrefabFetcher prefabFetcher,
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer,
            ISkirmishModel skirmish) 
            : base(appModel, prefabFetcher, prefabFactory, deferrer)
        {
            Assert.IsNotNull(skirmish);

            _random = RandomGenerator.Instance;
            _skirmish = skirmish;
        }

        public override ILevel GetLevel()
        {
            int levelNum = -99;  // Unused for skirmish
            SoundKeyPair musicKeys = _random.RandomItem(SoundKeys.Music.Background.All);
            string skyMaterialName = _random.RandomItem(SkyMaterials.All);

            return
                new Level(
                    levelNum,
                    _skirmish.AICruiser,
                    musicKeys,
                    skyMaterialName);
        }

        protected override IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            return new SkirmishStrategyFactory(_skirmish.AIStrategy);
        }

        public override Task<string> GetEnemyNameAsync(int levelNum)
        {
            return Task.FromResult("SKIRMISH");
        }

        public override async Task<IPrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            int randomLevelNum = _random.Range(1, StaticData.NUM_OF_LEVELS);
            return await _backgroundStatsProvider.GetStatsAsync(randomLevelNum);
        }

        public override IPrefabKey GetAiCruiserKey()
        {
            return _skirmish.AICruiser;
        }

        protected override Difficulty FindDifficulty()
        {
            return _skirmish.Difficulty;
        }
    }
}