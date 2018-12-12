using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.ClickSteps
{
    public class SlotsStepTests : TutorialStepTestsBase
    {
        private ITutorialStep _clickStep;
        private ISlotPermitter _permitter;
        private ISlot _slot;
        private ISlotProvider _slotProvider;
        private IItemProvider<IClickableEmitter> _clickableProvider;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _permitter = Substitute.For<ISlotPermitter>();

            _slot = Substitute.For<ISlot>();
            _slotProvider = Substitute.For<ISlotProvider>();

            _clickableProvider = _slotProvider;
            _clickableProvider.FindItem().Returns(_slot);

            _clickStep = new SlotStep(_args, _permitter, _slotProvider);
        }

        [Test]
        public void Click_DisablesSlots()
        {
            _clickStep.Start(_completionCallback);

            _slot.Clicked += Raise.Event();
            _permitter.Received().PermittedSlots = null;
        }
    }
}
