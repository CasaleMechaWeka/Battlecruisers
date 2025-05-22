using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.EnemyCruiser
{
    public class StartConstructingUnitStepTests : TutorialStepTestsBase
    {
        private TutorialStep _tutorialStep;
        private IPrefabKey _unitToConstruct;
        private IItemProvider<IFactory> _factoryProvider;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _unitToConstruct = Substitute.For<IPrefabKey>();
            _factoryProvider = Substitute.For<IItemProvider<IFactory>>();

            _tutorialStep = new StartConstructingUnitStep(_args, _unitToConstruct, _factoryProvider);
        }

        [Test]
        public void Start_ConstructsUnit_AndCompletes()
        {
            IBuildableWrapper<IUnit> buildableWrapper = Substitute.For<IBuildableWrapper<IUnit>>();

            PrefabFactory
                .GetUnitWrapperPrefab(_unitToConstruct)
                .Returns(buildableWrapper);

            IFactory factory = Substitute.For<IFactory>();
            _factoryProvider.FindItem().Returns(factory);

            _tutorialStep.Start(_completionCallback);

            factory.Received().StartBuildingUnit(buildableWrapper);

            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
