using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Scenes.BattleScene
{
    /// <summary>
    /// + Music
    /// + Alerts (eg: cruiser under attack, enemy is building an ultra)
    /// </summary>
    public class AudioInitialiser
    {
        private DangerMusicPlayer _dangerMusicPlayer;
        private IManagedDisposable _droneEventSoundPlayer;
        private readonly CruiserEventMonitor _cruiserEventMonitor;
        private UltrasConstructionMonitor _ultrasConstructionMonitor;

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
                    new CruiserDamagedMonitorDebouncer(
                        new CruiserDamageMonitor(playerCruiser),
                        time),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
        }

        private UltrasConstructionMonitor CreateUltrasConstructionMonitor(ICruiser aiCruiser)
        {
            return
                new UltrasConstructionMonitor(
                    aiCruiser,
                    aiCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
        }
    }
}