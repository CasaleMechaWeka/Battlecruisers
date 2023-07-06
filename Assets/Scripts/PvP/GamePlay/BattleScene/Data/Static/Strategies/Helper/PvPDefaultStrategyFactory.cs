using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Helper;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper
{
    public class PvPDefaultStrategyFactory : IPvPStrategyFactory
    {
        private readonly ILevelStrategies _levelStrategies;
        private readonly int _levelNum;

        public PvPDefaultStrategyFactory(ILevelStrategies levelStrategies, int levelNum)
        {
            Assert.IsNotNull(levelStrategies);

            _levelStrategies = levelStrategies;
            _levelNum = levelNum;
        }

        public IStrategy GetAdaptiveStrategy()
        {
            return _levelStrategies.GetAdaptiveStrategy(_levelNum);
        }

        public IStrategy GetBasicStrategy()
        {
            return _levelStrategies.GetBasicStrategy(_levelNum);
        }
    }
}