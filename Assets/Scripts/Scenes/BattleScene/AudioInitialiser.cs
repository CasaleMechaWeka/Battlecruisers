using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Scenes.BattleScene
{
    /// <summary>
    /// + Music
    /// + Alerts (eg: cruiser under attack, enemy is building an ultra)
    /// </summary>
    public class AudioInitialiser
    {
        private readonly DangerMusicPlayer _dangerMusicPlayer;
        private readonly IManagedDisposable _droneEventSoundPlayer;
        private readonly CruiserEventMonitor _cruiserEventMonitor;
        private readonly UltrasConstructionMonitor _ultrasConstructionMonitor;
        private readonly PopulationLimitAnnouncer _populationLimitAnnouncer;

        public AudioInitialiser(
            IBattleSceneHelper helper,
            IMusicPlayer musicPlayer,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IDeferrer deferrer,
            ITime time)
        {
            Helper.AssertIsNotNull(helper, musicPlayer, playerCruiser, aiCruiser, deferrer, time);

            _dangerMusicPlayer = CreateDangerMusicPlayer(musicPlayer, playerCruiser, aiCruiser, deferrer);
            _droneEventSoundPlayer = helper.CreateDroneEventSoundPlayer(playerCruiser, deferrer);
            _cruiserEventMonitor = CreateCruiserEventMonitor(playerCruiser, time);
            _ultrasConstructionMonitor = CreateUltrasConstructionMonitor(aiCruiser);
            _populationLimitAnnouncer = CreatePopulationLimitAnnouncer(playerCruiser, time);
        }

        private DangerMusicPlayer CreateDangerMusicPlayer(
            IMusicPlayer musicPlayer,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IDeferrer deferrer)
        {
            return
                new DangerMusicPlayer(
                    musicPlayer,
                    new DangerMonitor(
                        playerCruiser,
                        aiCruiser,
                        new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                        new HealthThresholdMonitor(aiCruiser, thresholdProportion: 0.3f)),
                    deferrer);
        }

        private CruiserEventMonitor CreateCruiserEventMonitor(ICruiser playerCruiser, ITime time)
        {
            return
                new CruiserEventMonitor(
                    new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
                    new CruiserDamageMonitor(playerCruiser),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time, debounceTimeInS: 20));
        }

        private UltrasConstructionMonitor CreateUltrasConstructionMonitor(ICruiser aiCruiser)
        {
            return
                new UltrasConstructionMonitor(
                    aiCruiser,
                    aiCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
        }

        private PopulationLimitAnnouncer CreatePopulationLimitAnnouncer(ICruiser playerCruiser, ITime time)
        {
            return
                new PopulationLimitAnnouncer(
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(time, debounceTimeInS: 30),
                    playerCruiser.PopulationLimitMonitor);
        }
    }
}