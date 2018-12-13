using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.EnemyCruiser
{
    public class StartConstructingBuildingStepTests : TutorialStepTestsBase
    {
        private StartConstructingBuildingStep _tutorialStep;
        private IPrefabKey _buildingToConstruct;
        private IPrefabFactory _prefabFactory;
        private ICruiserController _parentCruiser;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _buildingToConstruct = Substitute.For<IPrefabKey>();
            _prefabFactory = Substitute.For<IPrefabFactory>();
            _parentCruiser = Substitute.For<ICruiserController>();

            _tutorialStep = new StartConstructingBuildingStep(_args, _buildingToConstruct, _prefabFactory, _parentCruiser);
        }

        [Test]
        public void Start_ConstructsBuilding_AndCompletes()
        {
            IBuildableWrapper<IBuilding> buildingWrapper = Substitute.For<IBuildableWrapper<IBuilding>>();
            _prefabFactory
                .GetBuildingWrapperPrefab(_buildingToConstruct)
                .Returns(buildingWrapper);

            _parentCruiser.SlotAccessor
                .IsSlotAvailable(buildingWrapper.Buildable.SlotSpecification)
                .Returns(true);

            ISlot slot = Substitute.For<ISlot>();
            _parentCruiser.SlotAccessor
                .GetFreeSlot(buildingWrapper.Buildable.SlotSpecification)
                .Returns(slot);

            IBuilding building = Substitute.For<IBuilding>();
            _parentCruiser
                .ConstructBuilding(buildingWrapper.UnityObject, slot)
                .Returns(building);

            _tutorialStep.Start(_completionCallback);

            _prefabFactory.Received().GetBuildingWrapperPrefab(_buildingToConstruct);
            _parentCruiser.SlotAccessor.Received().IsSlotAvailable(buildingWrapper.Buildable.SlotSpecification);
            _parentCruiser.SlotAccessor.Received().GetFreeSlot(buildingWrapper.Buildable.SlotSpecification);
            _parentCruiser.Received().ConstructBuilding(buildingWrapper.UnityObject, slot);

            Assert.AreSame(building, _tutorialStep.FindItem());
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
