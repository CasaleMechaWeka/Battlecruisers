using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Helper;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper
{
    public class PvPDefaultStrategyFactory : IPvPStrategyFactory
    {
        private readonly IPvPLevelStrategies _levelStrategies;
        private readonly int _levelNum;

        public PvPDefaultStrategyFactory(IPvPLevelStrategies levelStrategies, int levelNum)
        {
            Assert.IsNotNull(levelStrategies);

            _levelStrategies = levelStrategies;
            _levelNum = levelNum;
        }

        public IPvPStrategy GetAdaptiveStrategy()
        {
            return _levelStrategies.GetAdaptiveStrategy(_levelNum);
        }

        public IPvPStrategy GetBasicStrategy()
        {
            return _levelStrategies.GetBasicStrategy(_levelNum);
        }
    }
}