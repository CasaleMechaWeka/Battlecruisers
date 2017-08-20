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
        private readonly IBuildingKeyHelper _buildingKeyHelper;
        private readonly IList<IPrefabKey> _availableBuildings;

        public IPrefabKey Current { get; private set; }

        public InfiniteBuildOrder(
            BuildingCategory buildingCategory, 
            IBuildingKeyHelper buildingKeyHelper)
		{
            Assert.IsNotNull(buildingKeyHelper);

            _buildingKeyHelper = buildingKeyHelper;

            _availableBuildings = _buildingKeyHelper.GetAvailableBuildings(buildingCategory);
            Assert.IsTrue(_availableBuildings.Count != 0);
		}

        public bool MoveNext()
        {
            Current
                = _availableBuildings
                    .Where(buildingKey => _buildingKeyHelper.CanConstructBuilding(buildingKey))
                    .Shuffle()
                    .FirstOrDefault();
            
            return Current != null;
        }
    }
}
