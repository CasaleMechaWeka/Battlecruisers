using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class SlotsStep : ClickStep
    {
        private readonly IList<ISlot> _slots;
        private readonly ISlotPermitter _highlightableSlotPermitter;

        public SlotsStep(
            ITutorialStepArgs args, 
			ISlotPermitter highlightableSlotPermitter,
            params ISlot[] slots)
            : base(args, new StaticListProvider<IClickable>(slots))
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
