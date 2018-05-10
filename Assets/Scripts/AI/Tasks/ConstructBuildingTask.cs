using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
{
    public class ConstructBuildingTask : IInternalTask
    {
        private readonly IPrefabKey _key;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;
        private readonly IDeferrer _deferrer;

        private IBuildable _building;
		
		public event EventHandler Completed;

        public ConstructBuildingTask(IPrefabKey key, IPrefabFactory prefabFactory, ICruiserController cruiser, IDeferrer deferrer) 
        {
            _key = key;
            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
            _deferrer = deferrer;
        }

        public void Start()
        {
            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_key);

            Assert.IsTrue(buildingWrapperPrefab.Buildable.NumOfDronesRequired <= _cruiser.DroneManager.NumOfDrones, 
                "Cannot afford to construct building " + buildingWrapperPrefab.Buildable.Name + " need " +
                buildingWrapperPrefab.Buildable.NumOfDronesRequired + " but only have " + _cruiser.DroneManager.NumOfDrones);

            if (_cruiser.SlotWrapper.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotType))
            {
                ISlot slot = _cruiser.SlotWrapper.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotType, buildingWrapperPrefab.Buildable.PreferCruiserFront);
				Assert.IsNotNull(slot);
				
                _building = _cruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
				_building.CompletedBuildable += Building_CompletedBuildable;
            }
            else
            {
                // Cruiser has no available slot for this building.  Task is completed.
                // Defer to frame end to allow this callstack to unravel.  This means InProgressState.OnCompleted
                // is called (good) instead of InitialState.OnCompleted() being called (bad).
                _deferrer.Defer(EmitCompletedEvent);
            }
        }

        public void Stop()
        {
            // Empty
        }

		public void Resume()
		{
			// Emtpy
		}

		private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            _building.CompletedBuildable -= Building_CompletedBuildable;
            EmitCompletedEvent();
        }

        private void EmitCompletedEvent()
        {
            if (Completed != null)
            {
                Completed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
