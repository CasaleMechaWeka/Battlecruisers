using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using System;

namespace BattleCruisers.Cruisers.Damage
{
    /// <summary>
    /// Plays sounds to the user when:
    /// 1. The cruiser (or its buildings) are damaged
    /// 2. The cruiser reaches critical health (say a third)
    /// </summary>
    public class CruiserEventMonitor : IManagedDisposable
    {
        private readonly IHealthThresholdMonitor _cruiserHealthThresholdMonitor;
        private readonly ICruiserDamageMonitor _cruiserDamageMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;
        private readonly IDebouncer _damagedDebouncer;

        public CruiserEventMonitor(
            IHealthThresholdMonitor cruiserHealthThresholdMonitor,
            ICruiserDamageMonitor cruiserDamageMonitor,
            IPrioritisedSoundPlayer soundPlayer,
            IDebouncer damagedDebouncer)
        {
            Helper.AssertIsNotNull(cruiserHealthThresholdMonitor, cruiserDamageMonitor, soundPlayer, damagedDebouncer);

            _cruiserHealthThresholdMonitor = cruiserHealthThresholdMonitor;
            _cruiserDamageMonitor = cruiserDamageMonitor;
            _soundPlayer = soundPlayer;
            _damagedDebouncer = damagedDebouncer;

            _cruiserHealthThresholdMonitor.DroppedBelowThreshold += _cruiserHealthThresholdMonitor_DroppedBelowThreshold;
            _cruiserDamageMonitor.CruiserOrBuildingDamaged += _cruiserDamageMonitor_CruiserOrBuildingDamaged;
        }

        private void _cruiserHealthThresholdMonitor_DroppedBelowThreshold(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Cruiser.SignificantlyDamaged);
        }

        private void _cruiserDamageMonitor_CruiserOrBuildingDamaged(object sender, EventArgs e)
        {
            _damagedDebouncer.Debounce(() => _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Cruiser.UnderAttack));
        }

        public void DisposeManagedState()
        {
            _cruiserHealthThresholdMonitor.DroppedBelowThreshold -= _cruiserHealthThresholdMonitor_DroppedBelowThreshold;
            _cruiserDamageMonitor.CruiserOrBuildingDamaged -= _cruiserDamageMonitor_CruiserOrBuildingDamaged;
        }
    }
}