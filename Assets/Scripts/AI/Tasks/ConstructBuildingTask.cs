using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
{
    public class ConstructBuildingTask : Task
    {
        private readonly IPrefabKey _key;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _cruiser;

        // FELIX  NEXT  Create IBuilding :D
        private Building _building;

        public ConstructBuildingTask(TaskPriority priority, IPrefabKey key, IPrefabFactory prefabFactory, ICruiserController cruiser) 
            : base(priority)
        {
            _key = key;
            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
        }

        // FELIX  States to avoid branching?
        public override void Start()
        {
            base.Start();

            if (_isCompleted)
            {
                return;
            }

            IBuildableWrapper<Building> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_key);

            if (_cruiser.IsSlotAvailable(buildingWrapperPrefab.Buildable.slotType))
            {
                // Cruiser has no available slot for this building.  Task is completed (perhaps with a failure result?).
                _isCompleted = true;
                EmitCompletedEvent();
            }
            else
            {
                ISlot slot = _cruiser.GetFreeSlot(buildingWrapperPrefab.Buildable.slotType);
				Assert.IsNotNull(slot);
				
                // FELIX  Ugly!  Make cruiser use the type of UnityObject, to avoid cast!!
				_building = _cruiser.ConstructBuilding((BuildingWrapper)buildingWrapperPrefab.UnityObject, slot);
                _building.CompletedBuildable += Building_CompletedBuildable;
            }
        }

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            _building.CompletedBuildable -= Building_CompletedBuildable;
            _isCompleted = true;
            EmitCompletedEvent();
        }
    }
}
