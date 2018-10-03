using System;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    /// <summary>
    /// Must be used in tandem with BuildingButtonStep.
    /// </summary>
    public class SlotsStep : ClickStep
    {
        private readonly ISlotPermitter _highlightableSlotPermitter;

        public SlotsStep(
            ITutorialStepArgs args, 
			ISlotPermitter highlightableSlotPermitter,
            ISlotsProvider slotsProvider)
            : base(args, slotsProvider)
        {
            Assert.IsNotNull(highlightableSlotPermitter);
            _highlightableSlotPermitter = highlightableSlotPermitter;
        }

		protected override void OnCompleted()
		{
            _highlightableSlotPermitter.PermittedSlots = null;

			base.OnCompleted();
		}
	}
}
