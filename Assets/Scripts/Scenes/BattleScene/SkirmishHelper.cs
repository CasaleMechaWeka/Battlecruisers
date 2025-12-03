using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils.Threading;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        private readonly SkirmishModel _skirmish;

        public override bool ShowInGameHints => false;
        public override IPrefabKey PlayerCruiser => _skirmish.PlayerCruiser;

        public SkirmishHelper(IDeferrer deferrer)
            : base(deferrer)
        {
            _skirmish = DataProvider.GameModel.Skirmish;
            Assert.IsNotNull(_skirmish);
        }

        public override Level GetLevel()
        {
            int levelNum = 1;  // Unused for skirmish

            // Use base.GetLevel() approach but modify for our needs
            ApplicationModel.SelectedLevel = _skirmish.BackgroundLevelNum;
            Level backgroundLevel = base.GetLevel();

            return new Level(
                levelNum,
                _skirmish.AICruiser,
                backgroundLevel.MusicKeys,
                backgroundLevel.SkyMaterialName,
                StaticPrefabKeys.CaptainExos.GetCaptainExoKey(21));
        }

        protected override IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            bool canUseUltras = DataProvider.GameModel.UnlockedBuildings.Any(building => building.BuildingCategory == BuildingCategory.Ultra);
            return new SkirmishStrategyFactory(_skirmish.AIStrategy, canUseUltras);
        }

        public override Task<string> GetEnemyNameAsync(int levelNum)
        {
            return Task.FromResult("SIMULATRON");
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