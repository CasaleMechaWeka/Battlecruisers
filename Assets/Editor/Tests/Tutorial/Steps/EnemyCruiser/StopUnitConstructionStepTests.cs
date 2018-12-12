// FELIX  Convert or delete :)
//using BattleCruisers.Buildables.Buildings.Factories;
//using BattleCruisers.Tutorial.Providers;
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.EnemyCruiser;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.EnemyCruiser
//{
//    public class StopUnitConstructionStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _tutorialStep;
//        private IItemProvider<IFactory> _factoryProvider;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _factoryProvider = Substitute.For<IItemProvider<IFactory>>();

//            _tutorialStep = new StopUnitConstructionStep(_args, _factoryProvider);
//        }

//        [Test]
//        public void Start_StopsUnitConstruction_AndCompletes()
//        {
//            IFactory factory = Substitute.For<IFactory>();
//            _factoryProvider.FindItem().Returns(factory);

//            _tutorialStep.Start(_completionCallback);

//            factory.Received().StopBuildingUnit();

//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
