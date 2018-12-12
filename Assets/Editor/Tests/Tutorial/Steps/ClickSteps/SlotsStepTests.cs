// FELIX  Convert or delete :)
//using BattleCruisers.Cruisers.Slots;
//using BattleCruisers.Tutorial.Providers;
//using BattleCruisers.Tutorial.Steps;
//using BattleCruisers.Tutorial.Steps.ClickSteps;
//using BattleCruisers.Tutorial.Steps.Providers;
//using NSubstitute;
//using NUnit.Framework;

//namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
//{
//    public class SlotsStepTests : TutorialStepTestsBase
//    {
//        private ITutorialStep _clickStep;
//        private ISlotPermitter _permitter;
//        private ISlot _slot1, _slot2;
//        private ISlot[] _slots;
//        private ISlotsProvider _slotsProvider;
//        private IListProvider<IClickableEmitter> _clickablesProvider;

//        [SetUp]
//        public override void SetuUp()
//        {
//            base.SetuUp();

//            _permitter = Substitute.For<ISlotPermitter>();

//            _slot1 = Substitute.For<ISlot>();
//            _slot2 = Substitute.For<ISlot>();
//            _slots = new ISlot[]
//            {
//                _slot1,
//                _slot2
//            };
//            _slotsProvider = Substitute.For<ISlotsProvider>();

//            _clickablesProvider = _slotsProvider;
//            _clickablesProvider.FindItems().Returns(_slots);

//            _clickStep = new SlotsStep(_args, _permitter, _slotsProvider);
//        }

//        [Test]
//        public void Click_DisablesSlots()
//        {
//            _clickStep.Start(_completionCallback);

//            _slot2.Clicked += Raise.Event();
//            _permitter.Received().PermittedSlots = null;
//        }
//    }
//}
