//using System;
//using BattleCruisers.Buildables;
//using BattleCruisers.Buildables.Buildings;
//using BattleCruisers.Cruisers;
//using BattleCruisers.Data.PrefabKeys;
//using BattleCruisers.Fetchers;
//using UnityEngine.Assertions;

//namespace BattleCruisers.AI.Tasks
//{
//    public class ConstructBuildingTask : TaskController
//    {
//        private readonly IPrefabKey _key;
//        private readonly IPrefabFactory _prefabFactory;
//        private readonly ICruiserController _cruiser;

//        private IBuildable _building;

//        public ConstructBuildingTask(TaskPriority priority, IPrefabKey key, IPrefabFactory prefabFactory, ICruiserController cruiser) 
//            : base(priority)
//        {
//            _key = key;
//            _prefabFactory = prefabFactory;
//            _cruiser = cruiser;
//        }

//        // FELIX  States to avoid branching?
//        public override void Start()
//        {
//            base.Start();

//            if (_isCompleted)
//            {
//                return;
//            }

//            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_key);

//            if (_cruiser.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotType))
//            {
//				ISlot slot = _cruiser.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotType);
//				Assert.IsNotNull(slot);
				
//				_building = _cruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
//				_building.CompletedBuildable += Building_CompletedBuildable;
//            }
//            else
//            {
//				// Cruiser has no available slot for this building.  Task is completed (perhaps with a failure result?).
//				_isCompleted = true;
//				EmitCompletedEvent();
//            }
//        }

//        private void Building_CompletedBuildable(object sender, EventArgs e)
//        {
//            _building.CompletedBuildable -= Building_CompletedBuildable;
//            _isCompleted = true;
//            EmitCompletedEvent();
//        }
//    }
//}
