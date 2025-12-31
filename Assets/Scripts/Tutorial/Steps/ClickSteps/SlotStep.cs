using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Steps.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    /// <summary>
    /// Must be used in tandem with BuildingButtonStep.
    /// </summary>
    public class SlotStep : ExplanationClickStep
    {
        private readonly SpecificSlotsFilter _highlightableSlotPermitter;

        public SlotStep(
            TutorialStepArgs args,
            SpecificSlotsFilter highlightableSlotPermitter,
            SlotProvider slotProvider)
            : base(args, slotProvider)
        {
            Assert.IsNotNull(highlightableSlotPermitter);
            _highlightableSlotPermitter = highlightableSlotPermitter;
        }

        protected override void OnCompleted()
        {
            _highlightableSlotPermitter.PermittedSlot = null;

            base.OnCompleted();
        }
    }
}
