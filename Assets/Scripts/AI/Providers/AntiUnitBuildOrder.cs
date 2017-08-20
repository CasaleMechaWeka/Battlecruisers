using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    /// <summary>
    /// Always tries to provide the advanced defence key, as long as we can afford it.
    /// </summary>
    public class AntiUnitBuildOrder : IDynamicBuildOrder
    {
        private readonly IPrefabKey _basicDefenceKey, _advancedDefenceKey;
        private readonly IBuilding _basicDefenceBuilding, _advancedDefenceBuilding;
        private readonly IDroneManager _droneManager;
        private int _numOfSlotsToUse, _numOfSlotsUsed;

        public IPrefabKey Current { get; private set; }

        public AntiUnitBuildOrder(
            IPrefabKey basicDefenceKey,
            IPrefabKey advancedDefenceKey,
            IPrefabFactory prefabFactory,
            IDroneManager droneManager,
            int numOfSlotsToUse)
        {
            Helper.AssertIsNotNull(basicDefenceKey, advancedDefenceKey, prefabFactory, droneManager);
            Assert.IsTrue(numOfSlotsToUse > 0);

            _basicDefenceKey = basicDefenceKey;
            _advancedDefenceKey = advancedDefenceKey;
            _basicDefenceBuilding = prefabFactory.GetBuildingWrapperPrefab(_basicDefenceKey).Buildable;
            _advancedDefenceBuilding = prefabFactory.GetBuildingWrapperPrefab(_advancedDefenceKey).Buildable;
            _droneManager = droneManager;
            _numOfSlotsToUse = numOfSlotsToUse;
            _numOfSlotsUsed = 0;
        }

        public bool MoveNext()
        {
            bool haveKey = false;

            if (_numOfSlotsUsed < _numOfSlotsToUse)
            {
                if (CanAffordBuilding(_advancedDefenceBuilding))
                {
                    Current = _advancedDefenceKey;
                }
                else if (CanAffordBuilding(_basicDefenceBuilding))
                {
                    Current = _basicDefenceKey;
                }
                else
                {
                    throw new ArgumentException("Should always have enough drones to build the basic defence building :(");
                }
                haveKey = true;
            }
            else
            {
                Current = null;
            }

            return haveKey;
        }

        private bool CanAffordBuilding(IBuilding building)
        {
            return building.NumOfDronesRequired <= _droneManager.NumOfDrones;
        }
    }
}
