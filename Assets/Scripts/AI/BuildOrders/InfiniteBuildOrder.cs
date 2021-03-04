using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.BuildOrders
{
    public class InfiniteBuildOrder : IDynamicBuildOrder
	{
        private readonly ILevelInfo _levelInfo;
        private readonly IList<BuildingKey> _availableBuildings;

        public BuildingKey Current { get; private set; }

        public InfiniteBuildOrder(
            BuildingCategory buildingCategory, 
            ILevelInfo levelInfo,
            IList<BuildingKey> bannedBuildings)
		{
            Assert.IsNotNull(levelInfo);

            _levelInfo = levelInfo;

            _availableBuildings = _levelInfo.GetAvailableBuildings(buildingCategory);
            Assert.IsTrue(_availableBuildings.Count != 0, $"No available buildings for: {buildingCategory}");

            RemoveBuildingsToIgnore(bannedBuildings);
            Assert.IsTrue(_availableBuildings.Count != 0, $"No available buildings for: {buildingCategory}");
		}

        private void RemoveBuildingsToIgnore(IList<BuildingKey> bannedBuildings)
        {
            if (bannedBuildings == null)
            {
                return;
            }

            foreach (BuildingKey bannedBuilding in bannedBuildings)
            {
                _availableBuildings.Remove(bannedBuilding);
            }
        }

        public bool MoveNext()
        {
            Current
                = _availableBuildings
                    .Where(_levelInfo.CanConstructBuilding)
                    .Shuffle()
                    .FirstOrDefault();
            
            return Current != null;
        }
    }
}
