using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
{
    public class ConstructBuildingTask : IInternalTask
    {
        private readonly IPrefabKey _key;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;

        private IBuildable _building;
		
		public event EventHandler Completed;

        public ConstructBuildingTask(IPrefabKey key, IPrefabFactory prefabFactory, ICruiserController cruiser) 
        {
            _key = key;
            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
        }

        public void Start()
        {
            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_key);

            if (_cruiser.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotType))
            {
				ISlot slot = _cruiser.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotType);
				Assert.IsNotNull(slot);
				
                _building = _cruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
				_building.CompletedBuildable += Building_CompletedBuildable;
            }
            else
            {
                // FELIX  Will emit completed event BEFORE this callstack unravels, so before
                // we transition to the InProgress state.  And InitialState.OnCompleted throws :)
                // Perhaps dispatch?

				// Cruiser has no available slot for this building.  Task is completed (perhaps with a failure result?).
				EmitCompletedEvent();
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
