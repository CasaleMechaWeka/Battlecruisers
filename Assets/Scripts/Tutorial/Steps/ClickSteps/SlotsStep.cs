using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers.Slots;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    // FELIX  Test :)
    public class SlotsStep : ClickStep
    {
        private readonly IList<ISlot> _slots;
        private readonly ISlotPermitter _highlightableSlotPermitter;

        public SlotsStep(
            ITutorialStepArgs args, 
			ISlotPermitter highlightableSlotPermitter,
            params ISlot[] slots)
            : base(args, new StaticClickablesProvider(slots))
        {
            Assert.IsNotNull(highlightableSlotPermitter);

            _slots = slots;
            _highlightableSlotPermitter = highlightableSlotPermitter;
        }

		public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            _highlightableSlotPermitter.PermittedSlots = _slots;
		}

		protected override void OnCompleted()
		{
            _highlightableSlotPermitter.PermittedSlots = null;

			base.OnCompleted();
		}
	}
}
