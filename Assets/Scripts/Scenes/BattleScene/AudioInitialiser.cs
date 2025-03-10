using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Scenes.BattleScene
{
    /// <summary>
    /// + Music
    /// + Alerts (eg: cruiser under attack, enemy is building an ultra)
    /// </summary>
    public class AudioInitialiser
    {
        private readonly LevelMusicPlayer _levelMusicPlayer;
        private readonly IManagedDisposable _droneEventSoundPlayer;
        private readonly CruiserEventMonitor _cruiserEventMonitor;
        private readonly UltrasConstructionMonitor _ultrasConstructionMonitor;
        private readonly PopulationLimitAnnouncer _populationLimitAnnouncer;

        public AudioInitialiser(
            IBattleSceneHelper helper,
            ILayeredMusicPlayer musicPlayer,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IDeferrer deferrer,
            ITime time,
            IBattleCompletionHandler battleCompletionHandler,
            ICruiserDamageMonitor playerCruiserDamageMonitor,
            IGameObject popLimitReachedFeedback)
        {
            Helper.AssertIsNotNull(helper, musicPlayer, playerCruiser, aiCruiser, deferrer, time, battleCompletionHandler, playerCruiserDamageMonitor, popLimitReachedFeedback);

            _levelMusicPlayer = CreateLevelMusicPlayer(musicPlayer, playerCruiser, aiCruiser, deferrer, battleCompletionHandler);
            _droneEventSoundPlayer = helper.CreateDroneEventSoundPlayer(playerCruiser, deferrer);
            _cruiserEventMonitor = CreateCruiserEventMonitor(playerCruiser, time, playerCruiserDamageMonitor);
            _ultrasConstructionMonitor = CreateUltrasConstructionMonitor(aiCruiser, time);
            _populationLimitAnnouncer = CreatePopulationLimitAnnouncer(playerCruiser, time, popLimitReachedFeedback);
        }

        private LevelMusicPlayer CreateLevelMusicPlayer(
            ILayeredMusicPlayer musicPlayer,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IDeferrer deferrer,
            IBattleCompletionHandler battleCompletionHandler)
        {
            IDangerMonitor dangerMonitor 
                = new DangerMonitor(
                    deferrer,
                    playerCruiser,
                    aiCruiser,
                    new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    new HealthThresholdMonitor(aiCruiser, thresholdProportion: 0.3f));
            return
                new LevelMusicPlayer(
                    musicPlayer,
                    new DangerMonitorSummariser(dangerMonitor),
                    battleCompletionHandler);
        }

        private CruiserEventMonitor CreateCruiserEventMonitor(
            ICruiser playerCruiser, 
            ITime time,
            ICruiserDamageMonitor playerCruiserDamageMonitor)
        {
            return
                new CruiserEventMonitor(
                    new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    playerCruiserDamageMonitor,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30));
        }

        private UltrasConstructionMonitor CreateUltrasConstructionMonitor(ICruiser aiCruiser, ITime time)
        {
            return
                new UltrasConstructionMonitor(
                    aiCruiser,
                    aiCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30));
        }

        private PopulationLimitAnnouncer CreatePopulationLimitAnnouncer(ICruiser playerCruiser, ITime time, IGameObject popLimitReachedFeedback)
        {
            return
                new PopulationLimitAnnouncer(
                    playerCruiser.PopulationLimitMonitor,
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time.RealTimeSinceGameStartProvider, debounceTimeInS: 30),
                    popLimitReachedFeedback);
        }
    }
}