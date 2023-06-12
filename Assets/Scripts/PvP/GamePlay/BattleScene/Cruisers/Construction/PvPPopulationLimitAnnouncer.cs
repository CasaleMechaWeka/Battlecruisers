using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPPopulationLimitAnnouncer
    {
        private PvPCruiser _playerCruiser;
        private readonly IPvPPopulationLimitMonitor _populationLimitMonitor;
        private readonly IPvPPrioritisedSoundPlayer _soundPlayer;
        private readonly IPvPDebouncer _debouncer;
        private readonly IPvPGameObject _popLimitReachedFeedback;

        public PvPPopulationLimitAnnouncer(
            PvPCruiser playerCruiser,
            IPvPPopulationLimitMonitor populationLimitMonitor        
            )
        {
            PvPHelper.AssertIsNotNull(populationLimitMonitor);

            _playerCruiser = playerCruiser;
            _populationLimitMonitor = populationLimitMonitor;     
            _populationLimitMonitor.IsPopulationLimitReached.ValueChanged += IsPopulationLimitReached_ValueChanged;
        }

        private void IsPopulationLimitReached_ValueChanged(object sender, EventArgs e)
        {
            _playerCruiser.pvp_popLimitReachedFeedback.Value = _populationLimitMonitor.IsPopulationLimitReached.Value;            
        }
    }
}