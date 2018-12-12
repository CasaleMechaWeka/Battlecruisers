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
        private readonly ISlotPermitter _highlightableSlotPermitter;

        public SlotStep(
            ITutorialStepArgsNEW args, 
			ISlotPermitter highlightableSlotPermitter,
            ISlotProvider slotProvider)
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
