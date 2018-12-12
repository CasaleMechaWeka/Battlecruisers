//using BattleCruisers.Buildables;
//using BattleCruisers.Tutorial.Providers;
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.WaitSteps;
//using NSubstitute;
//using NUnit.Framework;
//using UnityAsserts = UnityEngine.Assertions;

//namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
//{
//    // FELIX  Convert or delete :)
//    public class TargetDestroyedWaitStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _tutorialStep;
//        private IItemProvider<ITarget> _targetProvider;
//        private ITarget _target;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _targetProvider = Substitute.For<IItemProvider<ITarget>>();
//            _target = Substitute.For<ITarget>();
//            _targetProvider.FindItem().Returns(_target);

//            _tutorialStep = new TargetDestroyedWaitStep(_args, _targetProvider);
//        }

//        #region Start
//        [Test]
//        public void Start()
//        {
//            _tutorialStep.Start(_completionCallback);
//            _targetProvider.Received().FindItem();
//        }

//        [Test]
//        public void Start_NullTargetProvided_Throws()
//        {
//            _targetProvider.FindItem().Returns((ITarget)null);
//            Assert.Throws<UnityAsserts.AssertionException>(() => _tutorialStep.Start(_completionCallback));
//        }
//        #endregion Start

//        [Test]
//        public void TargetDestroyed_TriggersCompletedCallback()
//        {
//            Start();

//            _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target));
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
