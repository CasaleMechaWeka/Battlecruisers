using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Providers.Strategies.Requests;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    // FELIX  Tied to specific level.  Rename to LevelHelper?  LevelWrapper?
    // FELIX  Test!
    public class BuildingKeyHelper : IBuildingKeyHelper
	{
		private readonly IDroneManager _droneManager;
		private readonly IStaticData _staticData;
        private readonly IPrefabFactory _prefabFactory;
        private readonly int _levelNum;

        public BuildingKeyHelper(
            IDroneManager droneManager,
            IStaticData staticData,
            IPrefabFactory prefabFactory,
            int levelNum)
        {
            Helper.AssertIsNotNull(droneManager, staticData, prefabFactory);

            _droneManager = droneManager;
            _staticData = staticData;
            _prefabFactory = prefabFactory;
            _levelNum = levelNum;
        }

		public bool CanConstructBuilding(IPrefabKey buildingKey)
        {
            IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey).Buildable;

			return
				_staticData.IsBuildableAvailable(buildingKey, _levelNum)
				&& building.NumOfDronesRequired <= _droneManager.NumOfDrones;
        }

        public IList<IPrefabKey> GetAvailableBuildings(BuildingCategory category)
        {
            return _staticData.GetAvailableBuildings(category, _levelNum);
        }

		/// <summary>
		/// 1. High focus requests are served once before low focus requests
		/// 2. Low focus request get one slot at most
		/// 3. Remaining slots are split evenly between high focus requests
		/// </summary>
        public void AssignSlots(IEnumerable<IOffensiveRequest> slotRequests, int numOfSlotsAvailable)
		{
			IEnumerable<IOffensiveRequest> highFocusRequests = slotRequests.Where(request => request.Focus == OffensiveFocus.High);
			IEnumerable<IOffensiveRequest> lowFocusRequest = slotRequests.Where(request => request.Focus == OffensiveFocus.Low);

			int numOfSlotsUsed = 0;

			// Assign a round of high focus requests
			numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, numOfSlotsAvailable, numOfSlotsUsed);

			// Assign a round of low focus request
			numOfSlotsUsed = AssignSlotsToRequests(lowFocusRequest, numOfSlotsAvailable, numOfSlotsUsed);

			// Assign remaining slots to high focus requests
			if (highFocusRequests.Any())
			{
				while (numOfSlotsUsed < numOfSlotsAvailable)
				{
					numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, numOfSlotsAvailable, numOfSlotsUsed);
				}
			}
		}

		private int AssignSlotsToRequests(IEnumerable<IOffensiveRequest> requests, int numOfPlatformSlots, int numOfSlotsUsed)
		{
			foreach (IOffensiveRequest request in requests)
			{
				if (numOfSlotsUsed < numOfPlatformSlots)
				{
					request.NumOfSlotsToUse++;
					numOfSlotsUsed++;
				}
				else
				{
					break;
				}
			}

			return numOfSlotsUsed;
		}
    }
}
