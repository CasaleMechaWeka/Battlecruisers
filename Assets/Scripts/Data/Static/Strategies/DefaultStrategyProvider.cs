using BattleCruisers.Data.Static.Strategies;
using UnityEngine.Assertions;

namespace Assets.Scripts.Data.Static.Strategies
{
    public class DefaultStrategyProvider : IStrategyProvider
    {
        private readonly ILevelStrategies _levelStrategies;
        private readonly int _levelNum;

        public DefaultStrategyProvider(ILevelStrategies levelStrategies, int levelNum)
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