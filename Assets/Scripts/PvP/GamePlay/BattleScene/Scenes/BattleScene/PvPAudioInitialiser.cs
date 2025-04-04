using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    /// <summary>
    /// + Music
    /// + Alerts (eg: cruiser under attack, enemy is building an ultra)
    /// </summary>
    public class PvPAudioInitialiser
    {
        private readonly PvPLevelMusicPlayer _levelMusicPlayer;
        private readonly IManagedDisposable _droneEventSoundPlayer;
        private readonly CruiserEventMonitor _cruiserEventMonitor;
        private readonly PvPUltrasConstructionMonitor _ultrasConstructionMonitor;
        private readonly PvPPopulationLimitAnnouncer _populationLimitAnnouncer;

        public PvPAudioInitialiser(
            IPvPBattleSceneHelper helper,
            ILayeredMusicPlayer musicPlayer,
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IDeferrer deferrer,
            ITime time,
            IPvPBattleCompletionHandler battleCompletionHandler,
            ICruiserDamageMonitor playerCruiserDamageMonitor,
            IGameObject popLimitReachedFeedback)
        {
            PvPHelper.AssertIsNotNull(helper, musicPlayer, playerCruiser, enemyCruiser, deferrer, time, battleCompletionHandler, playerCruiserDamageMonitor, popLimitReachedFeedback);

            _levelMusicPlayer = CreateLevelMusicPlayer(musicPlayer, playerCruiser, enemyCruiser, deferrer, battleCompletionHandler);
            //    _droneEventSoundPlayer = helper.CreateDroneEventSoundPlayer(playerCruiser, deferrer);
            _cruiserEventMonitor = CreateCruiserEventMonitor(playerCruiser, time, playerCruiserDamageMonitor);
            _ultrasConstructionMonitor = CreateUltrasConstructionMonitor(enemyCruiser, time);
            //    _populationLimitAnnouncer = CreatePopulationLimitAnnouncer(playerCruiser, time, popLimitReachedFeedback);
        }

        private PvPLevelMusicPlayer CreateLevelMusicPlayer(
            ILayeredMusicPlayer musicPlayer,
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IDeferrer deferrer,
            IPvPBattleCompletionHandler battleCompletionHandler)
        {
            IDangerMonitor dangerMonitor
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
                    new DangerMonitorSummariser(dangerMonitor),
                    battleCompletionHandler);
        }

        private CruiserEventMonitor CreateCruiserEventMonitor(
            PvPCruiser playerCruiser,
            ITime time,
            ICruiserDamageMonitor playerCruiserDamageMonitor)
        {
            return
                new CruiserEventMonitor(
                    new PvPHealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    playerCruiserDamageMonitor,
                    PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30));
        }

        private PvPUltrasConstructionMonitor CreateUltrasConstructionMonitor(IPvPCruiser aiCruiser, ITime time)
        {
            return
                new PvPUltrasConstructionMonitor(
                    aiCruiser,
                    PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30));
        }

        /*        private PvPPopulationLimitAnnouncer CreatePopulationLimitAnnouncer(PvPCruiser playerCruiser, ITime time, IGameObject popLimitReachedFeedback)
                {
                    return
                        new PvPPopulationLimitAnnouncer(
                            playerCruiser,
                            playerCruiser.PopulationLimitMonitor
        *//*                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                            new PvPDebouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30),
                            popLimitReachedFeedback*//*);
                }*/

        private PvPPopulationLimitAnnouncer CreatePopulationLimitAnnouncer(PvPCruiser playerCruiser, ITime time, IGameObject popLimitReachedFeedback)
        {
            return
                new PvPPopulationLimitAnnouncer(
                    playerCruiser.PopulationLimitMonitor,
                    PvPFactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30),
                    popLimitReachedFeedback);
        }
    }
}