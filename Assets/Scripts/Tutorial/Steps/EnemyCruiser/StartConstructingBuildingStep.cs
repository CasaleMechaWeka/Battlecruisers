using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class StartConstructingBuildingStep : TutorialStep, IItemProvider<IBuildable>
    {
        private readonly IPrefabKey _buildingToConstruct;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _parentCruiser;

        private IBuilding _building;

        public StartConstructingBuildingStep(
            ITutorialStepArgs args, 
            IPrefabKey buildingToConstruct, 
            IPrefabFactory prefabFactory, 
            ICruiserController parentCruiser) 
            : base(args)
        {
            Helper.AssertIsNotNull(buildingToConstruct, prefabFactory, parentCruiser);

            _buildingToConstruct = buildingToConstruct;
            _prefabFactory = prefabFactory;
            _parentCruiser = parentCruiser;
        }

        public IBuildable FindItem()
        {
            return _building;
        }

        public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_buildingToConstruct);

            Assert.IsTrue(_parentCruiser.SlotAccessor.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotSpecification));
            ISlot slot = _parentCruiser.SlotAccessor.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotSpecification);

            _building = _parentCruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);

            OnCompleted();
		}
	}
}
