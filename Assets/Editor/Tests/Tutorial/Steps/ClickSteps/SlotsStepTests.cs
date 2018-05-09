using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class SlotsStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private ISlotPermitter _permitter;
        private ISlot _slot1, _slot2;
        private ISlot[] _slots;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _permitter = Substitute.For<ISlotPermitter>();

            _slot1 = Substitute.For<ISlot>();
            _slot2 = Substitute.For<ISlot>();
            _slots = new ISlot[]
            {
                _slot1,
                _slot2
            };

            _clickStep = new SlotsStep(_args, _permitter, _slots);
        }

        [Test]
        public void Start_PermitsSlots()
        {
            _clickStep.Start(_completionCallback);
            _permitter.Received().PermittedSlots = _slots;
        }

        [Test]
        public void Click_DisablesSlots()
        {
            Start_PermitsSlots();

            _slot2.Clicked += Raise.Event();
            _permitter.Received().PermittedSlots = null;
        }
    }
}
