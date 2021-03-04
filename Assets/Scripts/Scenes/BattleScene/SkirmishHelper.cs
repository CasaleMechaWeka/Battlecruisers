using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using UnityEngine.Assertions;

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
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer) 
            : base(appModel, prefabFetcher, prefabFactory, deferrer)
        {
            _skirmish = DataProvider.GameModel.Skirmish;
            Assert.IsNotNull(_skirmish);
        }

        public override ILevel GetLevel()
        {
            int levelNum = -99;  // Unused for skirmish
            ILevel backgroundLevel = _appModel.DataProvider.GetLevel(_skirmish.BackgroundLevelNum);

            return
                new Level(
                    levelNum,
                    _skirmish.AICruiser,
                    backgroundLevel.MusicKeys,
                    backgroundLevel.SkyMaterialName);
        }

        protected override IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            return new SkirmishStrategyFactory(_skirmish.AIStrategy);
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

        protected override Difficulty FindDifficulty()
        {
            return _skirmish.Difficulty;
        }
    }
}