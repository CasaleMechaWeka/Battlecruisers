using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public class SlotBoostFeedbackMonitor
    {
        private readonly Slot _parentSlot;
        private readonly BoostStateFinder _boostStateFinder;
        private readonly IBoostFeedback _boostFeedback;

        public SlotBoostFeedbackMonitor(Slot parentSlot, BoostStateFinder boostStateFinder, IBoostFeedback boostFeedback)
        {
            Helper.AssertIsNotNull(parentSlot, boostStateFinder, boostFeedback);

            _parentSlot = parentSlot;
            _boostStateFinder = boostStateFinder;
            _boostFeedback = boostFeedback;

            _parentSlot.Building.ValueChanged += (sender, e) => UpdateSlotBoostFeedback();
            _parentSlot.BoostProviders.CollectionChanged += (sender, e) => UpdateSlotBoostFeedback();
        }

        private void UpdateSlotBoostFeedback()
        {
            _boostFeedback.State = _boostStateFinder.FindState(_parentSlot.BoostProviders.Count, _parentSlot.Building.Value);
        }
    }
}