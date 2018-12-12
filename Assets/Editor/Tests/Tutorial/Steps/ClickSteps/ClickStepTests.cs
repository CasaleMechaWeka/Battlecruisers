// FELIX  Convert or delete :)
//using System.Collections.Generic;
//using BattleCruisers.Tutorial.Providers;
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.ClickSteps;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
//{
//    public class ClickStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _clickStep;
//        private IListProvider<IClickableEmitter> _clickablesProvider;
//        private IList<IClickableEmitter> _clickables;
//        private IClickableEmitter _clickable1, _clickable2;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _clickable1 = Substitute.For<IClickableEmitter>();
//            _clickable2 = Substitute.For<IClickableEmitter>();
//            _clickables = new List<IClickableEmitter>()
//            {
//                _clickable1, _clickable2
//            };
//            _clickablesProvider = Substitute.For<IListProvider<IClickableEmitter>>();
//            _clickablesProvider.FindItems().Returns(_clickables);

//            _clickStep = new ClickStep(_args, _clickablesProvider);
//        }

//        [Test]
//        public void Start()
//        {
//            _clickStep.Start(_completionCallback);
//            _clickablesProvider.Received().FindItems();
//        }

//        [Test]
//        public void ClickCompletes()
//        {
//            Start();

//            _clickable2.Clicked += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);
//        }

//        [Test]
//        public void SecondClick_DoesNothing()
//        {
//            Start();

//            // First click completes
//            _clickable2.Clicked += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);

//            // Second click does nothing
//            _clickable1.Clicked += Raise.Event();
//            Assert.AreEqual(1, _callbackCounter);
//        }
//    }
//}
