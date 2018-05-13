using System;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class SlotsStep : ClickStep
    {
        private readonly IListProvider<ISlot> _slotsProvider;
        private readonly ISlotPermitter _highlightableSlotPermitter;

        public SlotsStep(
            ITutorialStepArgs args, 
			ISlotPermitter highlightableSlotPermitter,
            ISlotsProvider slotsProvider)
            : base(args, slotsProvider)
        {
            Assert.IsNotNull(highlightableSlotPermitter);

            _slotsProvider = slotsProvider;
            _highlightableSlotPermitter = highlightableSlotPermitter;
        }

		public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            _highlightableSlotPermitter.PermittedSlots = _slotsProvider.FindItems();
		}

		protected override void OnCompleted()
		{
            _highlightableSlotPermitter.PermittedSlots = null;

			base.OnCompleted();
		}
	}
}
