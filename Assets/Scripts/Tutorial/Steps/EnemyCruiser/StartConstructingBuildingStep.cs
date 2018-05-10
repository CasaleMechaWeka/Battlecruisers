using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Write tests
    public class StartConstructingBuildingStep : TutorialStep, IProvider<IBuilding>
    {
        private readonly IPrefabKey _buildingToConstruct;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ICruiserController _parentCruiser;

        private IBuilding _building;

        public StartConstructingBuildingStep(
            ITutorialStepArgs args, 
            IPrefabKey buildingToconstruct, 
            IPrefabFactory prefabFactory, 
            ICruiserController parentCruiser) 
            : base(args)
        {
            Helper.AssertIsNotNull(buildingToconstruct, prefabFactory, parentCruiser);

            _buildingToConstruct = buildingToconstruct;
            _prefabFactory = prefabFactory;
            _parentCruiser = parentCruiser;
        }

		public IBuilding FindItem()
        {
            return _building;
        }

        public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            IBuildableWrapper<IBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_buildingToConstruct);

            Assert.IsTrue(_parentCruiser.SlotWrapper.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotType));
			ISlot slot = _parentCruiser.SlotWrapper.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotType, buildingWrapperPrefab.Buildable.PreferCruiserFront);

            _building = _parentCruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
		}
	}
}
