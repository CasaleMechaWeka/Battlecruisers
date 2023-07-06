using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    /// <summary>
    /// Plays sounds to the user when:
    /// 1. The cruiser (or its buildings) are damaged
    /// 2. The cruiser reaches critical health (say a third)
    /// </summary>
    public class PvPCruiserEventMonitor : IPvPManagedDisposable
    {
        private readonly IPvPHealthThresholdMonitor _cruiserHealthThresholdMonitor;
        private readonly IPvPCruiserDamageMonitor _cruiserDamageMonitor;
        private readonly IPvPPrioritisedSoundPlayer _soundPlayer;
        private readonly IPvPDebouncer _damagedDebouncer;

        public PvPCruiserEventMonitor(
            IPvPHealthThresholdMonitor cruiserHealthThresholdMonitor,
            IPvPCruiserDamageMonitor cruiserDamageMonitor,
            IPvPPrioritisedSoundPlayer soundPlayer,
            IPvPDebouncer damagedDebouncer)
        {
            PvPHelper.AssertIsNotNull(cruiserHealthThresholdMonitor, cruiserDamageMonitor, soundPlayer, damagedDebouncer);

            _cruiserHealthThresholdMonitor = cruiserHealthThresholdMonitor;
            _cruiserDamageMonitor = cruiserDamageMonitor;
            _soundPlayer = soundPlayer;
            _damagedDebouncer = damagedDebouncer;

            _cruiserHealthThresholdMonitor.DroppedBelowThreshold += _cruiserHealthThresholdMonitor_DroppedBelowThreshold;
            _cruiserDamageMonitor.CruiserOrBuildingDamaged += _cruiserDamageMonitor_CruiserOrBuildingDamaged;
        }

        private void _cruiserHealthThresholdMonitor_DroppedBelowThreshold(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPCruiser.SignificantlyDamaged);
        }

        private void _cruiserDamageMonitor_CruiserOrBuildingDamaged(object sender, EventArgs e)
        {
            _damagedDebouncer.Debounce(() => _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPCruiser.UnderAttack));
        }

        public void DisposeManagedState()
        {
            _cruiserHealthThresholdMonitor.DroppedBelowThreshold -= _cruiserHealthThresholdMonitor_DroppedBelowThreshold;
            _cruiserDamageMonitor.CruiserOrBuildingDamaged -= _cruiserDamageMonitor_CruiserOrBuildingDamaged;
        }
    }
}