using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public class InfiniteBuildOrder : IDynamicBuildOrder
	{
        private readonly ILevelInfo _levelInfo;
        private readonly IList<IPrefabKey> _availableBuildings;

        public IPrefabKey Current { get; private set; }

        public InfiniteBuildOrder(
            BuildingCategory buildingCategory, 
            ILevelInfo levelInfo)
		{
            Assert.IsNotNull(levelInfo);

            _levelInfo = levelInfo;

            _availableBuildings = _levelInfo.GetAvailableBuildings(buildingCategory);
            Assert.IsTrue(_availableBuildings.Count != 0);
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
