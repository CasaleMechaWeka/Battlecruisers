// FELIX  Convert or delete :)
//using System;
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.WaitSteps;
//using BattleCruisers.Utils.Threading;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.WaitSteps
//{
//    public class DelayWaitStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _tutorialStep;
//        private IVariableDelayDeferrer _deferrer;
//        private float _waitTimeInS;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _deferrer = Substitute.For<IVariableDelayDeferrer>();
//            _waitTimeInS = 12;
//            _tutorialStep = new DelayWaitStep(_args, _deferrer, _waitTimeInS);
//        }

//        [Test]
//        public void Start_DefersCompletionCallback()
//        {
//            _tutorialStep.Start(_completionCallback);
//            // Protected base class callback is passed
//            _deferrer.Received().Defer(Arg.Is<Action>(callback => true), _waitTimeInS);
//        }
//    }
//}
