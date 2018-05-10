using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
{
    public class ConstructBuildingTask : IInternalTask
    {
        private readonly IPrefabKey _buildingToConstruct;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _parentCruiser;
        private readonly IDeferrer _deferrer;

        private IBuildable _building;
		
		public event EventHandler Completed;

        public ConstructBuildingTask(IPrefabKey buildingToconstruct, IPrefabFactory prefabFactory, ICruiserController parentCruiser, IDeferrer deferrer) 
        {
            Helper.AssertIsNotNull(buildingToconstruct, prefabFactory, parentCruiser, deferrer);

            _buildingToConstruct = buildingToconstruct;
            _prefabFactory = prefabFactory;
            _parentCruiser = parentCruiser;
            _deferrer = deferrer;
        }

        public void Start()
        {
            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_buildingToConstruct);

            Assert.IsTrue(buildingWrapperPrefab.Buildable.NumOfDronesRequired <= _parentCruiser.DroneManager.NumOfDrones, 
                "Cannot afford to construct building " + buildingWrapperPrefab.Buildable.Name + " need " +
                buildingWrapperPrefab.Buildable.NumOfDronesRequired + " but only have " + _parentCruiser.DroneManager.NumOfDrones);

            if (_parentCruiser.SlotWrapper.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotType))
            {
                ISlot slot = _parentCruiser.SlotWrapper.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotType, buildingWrapperPrefab.Buildable.PreferCruiserFront);
				Assert.IsNotNull(slot);
				
                _building = _parentCruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
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
