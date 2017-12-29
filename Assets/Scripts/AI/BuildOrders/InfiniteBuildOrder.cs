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
        private readonly IList<IPrefabKey> _availableBuildings;

        public IPrefabKey Current { get; private set; }

        public InfiniteBuildOrder(
            BuildingCategory buildingCategory, 
            ILevelInfo levelInfo,
            IList<IPrefabKey> bannedBuildings)
		{
            Assert.IsNotNull(levelInfo);

            _levelInfo = levelInfo;

            _availableBuildings = _levelInfo.GetAvailableBuildings(buildingCategory);
            Assert.IsTrue(_availableBuildings.Count != 0);

            RemoveBuildingsToIgnore(bannedBuildings);
            Assert.IsTrue(_availableBuildings.Count != 0);
		}

        private void RemoveBuildingsToIgnore(IList<IPrefabKey> bannedBuildings)
        {
            if (bannedBuildings == null)
            {
                return;
            }

            foreach (IPrefabKey bannedBuilding in bannedBuildings)
            {
                _availableBuildings.Remove(bannedBuilding);
            }
        }

        public bool MoveNext()
        {
            Current
                = _availableBuildings
                    .Where(buildingKey => _levelInfo.CanConstructBuilding(buildingKey))
                    .Shuffle()
                    .FirstOrDefault();
            
            return Current != null;
        }
    }
}
