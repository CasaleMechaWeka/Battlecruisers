using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public class DefaultStrategyFactory : IStrategyFactory
    {
        private readonly ILevelStrategies _levelStrategies;
        private readonly ILevelStrategies _sideQuestStrategies;
        private readonly int _levelNum;
        private readonly bool _isSideQuest;

        public DefaultStrategyFactory(ILevelStrategies levelStrategies, ILevelStrategies sideQuestStrategies, int levelNum, bool isSideQuest = false)
        {
            Assert.IsNotNull(levelStrategies);

            _levelStrategies = levelStrategies;
            _sideQuestStrategies = sideQuestStrategies;
            _levelNum = levelNum;
            _isSideQuest = isSideQuest;
        }

        public Strategy GetAdaptiveStrategy()
        {
            if (_isSideQuest)
                return _sideQuestStrategies.GetAdaptiveStrategy(_levelNum);
            else
                return _levelStrategies.GetAdaptiveStrategy(_levelNum);
        }

        public Strategy GetBasicStrategy()
        {
            if (_isSideQuest)
                return _sideQuestStrategies.GetBasicStrategy(_levelNum);
            else
                return _levelStrategies.GetBasicStrategy(_levelNum);
        }
    }
}