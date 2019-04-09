using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
{
    public class ConstructBuildingTask : ITask
    {
        private readonly IPrefabKey _buildingToConstruct;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _parentCruiser;

        private IBuildable _building;
		
		public event EventHandler Completed;

        public ConstructBuildingTask(IPrefabKey buildingToconstruct, IPrefabFactory prefabFactory, ICruiserController parentCruiser) 
        {
            Helper.AssertIsNotNull(buildingToconstruct, prefabFactory, parentCruiser);

            _buildingToConstruct = buildingToconstruct;
            _prefabFactory = prefabFactory;
            _parentCruiser = parentCruiser;
        }

        public bool Start()
        {
            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_buildingToConstruct);

            bool haveStartedTask = false;

            if (_parentCruiser.IsAlive
                && _parentCruiser.SlotAccessor.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotSpecification))
            {
                Assert.IsTrue(buildingWrapperPrefab.Buildable.NumOfDronesRequired <= _parentCruiser.DroneManager.NumOfDrones, 
                    "Cannot afford to construct building " + buildingWrapperPrefab.Buildable.Name + " need " +
                    buildingWrapperPrefab.Buildable.NumOfDronesRequired + " but only have " + _parentCruiser.DroneManager.NumOfDrones);

                ISlot slot = _parentCruiser.SlotAccessor.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotSpecification);
				Assert.IsNotNull(slot);
				
                _building = _parentCruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
				_building.CompletedBuildable += _building_CompletedBuildable;
                _building.Destroyed += _building_Destroyed;

                haveStartedTask = true;
            }

            return haveStartedTask;
        }

        public void Stop()
        {
            // Empty
        }

		public void Resume()
		{
			// Emtpy
		}

		private void _building_CompletedBuildable(object sender, EventArgs e)
        {
            TaskCompleted();
        }

        private void _building_Destroyed(object sender, DestroyedEventArgs e)
        {
            TaskCompleted();
        }

        private void TaskCompleted()
        {
            _building.CompletedBuildable -= _building_CompletedBuildable;
            _building.Destroyed -= _building_Destroyed;

            EmitCompletedEvent();
        }

        private void EmitCompletedEvent()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return base.ToString() + "  building to consruct: " + _buildingToConstruct;
        }
    }
}
