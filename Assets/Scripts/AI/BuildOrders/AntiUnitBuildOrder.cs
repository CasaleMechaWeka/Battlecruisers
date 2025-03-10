using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.BuildOrders
{
    /// <summary>
    /// Provides the basic defense key for the very first key.
    /// 
    /// Afterwards always tries to provide the advanced defence key, as long as:
    /// 1. It is unlocked
    /// 2. AND we can afford it
    /// </summary>
    public class AntiUnitBuildOrder : IDynamicBuildOrder
    {
        private readonly BuildingKey _basicDefenceKey, _advancedDefenceKey;
        private readonly ILevelInfo _levelInfo;
		private readonly int _numOfSlotsToUse;
        private int _numOfSlotsUsed;
        private bool _isFirstKey;

        public BuildingKey Current { get; private set; }

        public AntiUnitBuildOrder(
            BuildingKey basicDefenceKey,
            BuildingKey advancedDefenceKey,
			ILevelInfo levelInfo,
            int numOfSlotsToUse)
        {
            Helper.AssertIsNotNull(basicDefenceKey, advancedDefenceKey, levelInfo);
            Assert.IsTrue(numOfSlotsToUse > 0);

            _basicDefenceKey = basicDefenceKey;
            _advancedDefenceKey = advancedDefenceKey;
            _levelInfo = levelInfo;
            _numOfSlotsToUse = numOfSlotsToUse;
            _numOfSlotsUsed = 0;
            _isFirstKey = true;
        }

        public bool MoveNext()
        {
            bool haveKey = false;

            if (_numOfSlotsUsed < _numOfSlotsToUse)
            {
                //Assert.IsTrue(_levelInfo.CanConstructBuilding(_basicDefenceKey), "Should always have enough drones to build the basic defence building :(");

                if (_isFirstKey
                    || !_levelInfo.CanConstructBuilding(_advancedDefenceKey))
                {
                    Current = _basicDefenceKey;
                    _isFirstKey = false;
                }
                else
                {
                    Current = _advancedDefenceKey;
                }

                _numOfSlotsUsed++;
                haveKey = true;
            }
            else
            {
                Current = null;
            }

            return haveKey;
        }
    }
}
