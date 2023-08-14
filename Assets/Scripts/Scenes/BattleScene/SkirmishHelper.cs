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
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        private readonly ISkirmishModel _skirmish;

        public override bool ShowInGameHints => false;
        public override IPrefabKey PlayerCruiser => _skirmish.PlayerCruiser;

        public SkirmishHelper(
            IApplicationModel appModel,
            IPrefabFetcher prefabFetcher,
            ILocTable storyStrings,
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer) 
            : base(appModel, prefabFetcher, storyStrings, prefabFactory, deferrer)
        {
            _skirmish = DataProvider.GameModel.Skirmish;
            Assert.IsNotNull(_skirmish);
        }

        public override ILevel GetLevel()
        {
            int levelNum = 1;  // Unused for skirmish
            ILevel backgroundLevel = _appModel.DataProvider.GetLevel(_skirmish.BackgroundLevelNum);

            return
                new Level(
                    levelNum,
                    _skirmish.AICruiser,
                    backgroundLevel.MusicKeys,
                    backgroundLevel.SkyMaterialName,
                    StaticPrefabKeys.CaptainExos.CaptainExo021);
        }

        protected override IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            bool canUseUltras = _appModel.DataProvider.GameModel.UnlockedBuildings.Any(building => building.BuildingCategory == BuildingCategory.Ultra);
            return new SkirmishStrategyFactory(_skirmish.AIStrategy, canUseUltras);
        }

        public override Task<string> GetEnemyNameAsync(int levelNum)
        {
            return Task.FromResult("SIMULATRON");
        }

        public override async Task<IPrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            return await _backgroundStatsProvider.GetStatsAsync(_skirmish.BackgroundLevelNum);
        }

        public override IPrefabKey GetAiCruiserKey()
        {
            return _skirmish.AICruiser;
        }

        public override IBuildProgressCalculator CreateAICruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreateAICruiserCalculator(FindDifficulty());
        }

        protected override Difficulty FindDifficulty()
        {
            return _skirmish.Difficulty;
        }
    }
}