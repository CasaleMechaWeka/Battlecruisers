using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    /// <summary>
    /// + Music
    /// + Alerts (eg: cruiser under attack, enemy is building an ultra)
    /// </summary>
    public class PvPAudioInitialiser
    {
        private readonly PvPLevelMusicPlayer _levelMusicPlayer;
        private readonly IPvPManagedDisposable _droneEventSoundPlayer;
        private readonly PvPCruiserEventMonitor _cruiserEventMonitor;
        private readonly PvPUltrasConstructionMonitor _ultrasConstructionMonitor;
        private readonly PvPPopulationLimitAnnouncer _populationLimitAnnouncer;

        public PvPAudioInitialiser(
            IPvPBattleSceneHelper helper,
            IPvPLayeredMusicPlayer musicPlayer,
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IPvPDeferrer deferrer,
            IPvPTime time,
            IPvPBattleCompletionHandler battleCompletionHandler,
            IPvPCruiserDamageMonitor playerCruiserDamageMonitor,
            IPvPGameObject popLimitReachedFeedback)
        {
            PvPHelper.AssertIsNotNull(helper, musicPlayer, playerCruiser, enemyCruiser, deferrer, time, battleCompletionHandler, playerCruiserDamageMonitor, popLimitReachedFeedback);

            _levelMusicPlayer = CreateLevelMusicPlayer(musicPlayer, playerCruiser, enemyCruiser, deferrer, battleCompletionHandler);
            _droneEventSoundPlayer = helper.CreateDroneEventSoundPlayer(playerCruiser, deferrer);
            _cruiserEventMonitor = CreateCruiserEventMonitor(playerCruiser, time, playerCruiserDamageMonitor);
            _ultrasConstructionMonitor = CreateUltrasConstructionMonitor(enemyCruiser, time);
            //    _populationLimitAnnouncer = CreatePopulationLimitAnnouncer(playerCruiser, time, popLimitReachedFeedback);
        }

        private PvPLevelMusicPlayer CreateLevelMusicPlayer(
            IPvPLayeredMusicPlayer musicPlayer,
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IPvPDeferrer deferrer,
            IPvPBattleCompletionHandler battleCompletionHandler)
        {
            IPvPDangerMonitor dangerMonitor
                = new PvPDangerMonitor
                (
                    deferrer,
                    playerCruiser,
                    enemyCruiser,
                    new PvPHealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    new PvPHealthThresholdMonitor(enemyCruiser, thresholdProportion: 0.3f));
            return
                new PvPLevelMusicPlayer(
                    musicPlayer,
                    new PvPDangerMonitorSummariser(dangerMonitor),
                    battleCompletionHandler);
        }

        private PvPCruiserEventMonitor CreateCruiserEventMonitor(
            PvPCruiser playerCruiser,
            IPvPTime time,
            IPvPCruiserDamageMonitor playerCruiserDamageMonitor)
        {
            return
                new PvPCruiserEventMonitor(
                    new PvPHealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    playerCruiserDamageMonitor,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new PvPDebouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30));
        }

        private PvPUltrasConstructionMonitor CreateUltrasConstructionMonitor(IPvPCruiser aiCruiser, IPvPTime time)
        {
            return
                new PvPUltrasConstructionMonitor(
                    aiCruiser,
                    aiCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new PvPDebouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30));
        }

        /*        private PvPPopulationLimitAnnouncer CreatePopulationLimitAnnouncer(PvPCruiser playerCruiser, IPvPTime time, IPvPGameObject popLimitReachedFeedback)
                {
                    return
                        new PvPPopulationLimitAnnouncer(
                            playerCruiser,
                            playerCruiser.PopulationLimitMonitor
        *//*                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                            new PvPDebouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30),
                            popLimitReachedFeedback*//*);
                }*/

        private PvPPopulationLimitAnnouncer CreatePopulationLimitAnnouncer(PvPCruiser playerCruiser, IPvPTime time, IPvPGameObject popLimitReachedFeedback)
        {
            return
                new PvPPopulationLimitAnnouncer(
                    playerCruiser.PopulationLimitMonitor,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new PvPDebouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30),
                    popLimitReachedFeedback);
        }
    }
}