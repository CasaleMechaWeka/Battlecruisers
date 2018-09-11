using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
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

        public CruiserEventMonitor(
            IHealthThresholdMonitor cruiserHealthThresholdMonitor,
            ICruiserDamageMonitor cruiserDamageMonitor,
            IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(cruiserHealthThresholdMonitor, cruiserDamageMonitor, soundPlayer);

            _cruiserHealthThresholdMonitor = cruiserHealthThresholdMonitor;
            _cruiserDamageMonitor = cruiserDamageMonitor;
            _soundPlayer = soundPlayer;

            _cruiserHealthThresholdMonitor.ThresholdReached += _cruiserHealthThresholdMonitor_ThresholdReached;
            _cruiserDamageMonitor.CruiserOrBuildingDamaged += _cruiserDamageMonitor_CruiserOrBuildingDamaged;
        }

        private void _cruiserHealthThresholdMonitor_ThresholdReached(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.CruiserSignificantlyDamaged);
        }

        private void _cruiserDamageMonitor_CruiserOrBuildingDamaged(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.CruiserUnderAttack);
        }

        public void DisposeManagedState()
        {
            _cruiserHealthThresholdMonitor.ThresholdReached -= _cruiserHealthThresholdMonitor_ThresholdReached;
            _cruiserDamageMonitor.CruiserOrBuildingDamaged -= _cruiserDamageMonitor_CruiserOrBuildingDamaged;
        }
    }
}