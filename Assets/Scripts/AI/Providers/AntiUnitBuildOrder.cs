using System;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    /// <summary>
    /// Always tries to provide the advanced defence key, as long as:
    /// 1. It is unlocked
    /// 2. AND we can afford it
    /// </summary>
    public class AntiUnitBuildOrder : IDynamicBuildOrder
    {
        private readonly IPrefabKey _basicDefenceKey, _advancedDefenceKey;
        private readonly IBuildingKeyHelper _buildingKeyHelper;
		private readonly int _numOfSlotsToUse;
        private int _numOfSlotsUsed;

        public IPrefabKey Current { get; private set; }

        public AntiUnitBuildOrder(
            IPrefabKey basicDefenceKey,
            IPrefabKey advancedDefenceKey,
			IBuildingKeyHelper buildingKeyHelper,
            int numOfSlotsToUse)
        {
            Helper.AssertIsNotNull(basicDefenceKey, advancedDefenceKey, buildingKeyHelper);
            Assert.IsTrue(numOfSlotsToUse > 0);

            _basicDefenceKey = basicDefenceKey;
            _advancedDefenceKey = advancedDefenceKey;
            _buildingKeyHelper = buildingKeyHelper;
            _numOfSlotsToUse = numOfSlotsToUse;
            _numOfSlotsUsed = 0;
        }

        public bool MoveNext()
        {
            bool haveKey = false;

            if (_numOfSlotsUsed < _numOfSlotsToUse)
            {
                if (_buildingKeyHelper.CanConstructBuilding(_advancedDefenceKey))
                {
                    Current = _advancedDefenceKey;
                }
                else if (_buildingKeyHelper.CanConstructBuilding(_basicDefenceKey))
                {
                    Current = _basicDefenceKey;
                }
                else
                {
                    throw new ArgumentException("Should always have enough drones to build the basic defence building :(");
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
