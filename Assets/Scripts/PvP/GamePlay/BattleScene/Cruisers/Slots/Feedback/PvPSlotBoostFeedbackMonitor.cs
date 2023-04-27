using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public class PvPSlotBoostFeedbackMonitor
    {
        private readonly IPvPSlot _parentSlot;
        private readonly IPvPBoostStateFinder _boostStateFinder;
        private readonly IPvPBoostFeedback _boostFeedback;

        public PvPSlotBoostFeedbackMonitor(IPvPSlot parentSlot, IPvPBoostStateFinder boostStateFinder, IPvPBoostFeedback boostFeedback)
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