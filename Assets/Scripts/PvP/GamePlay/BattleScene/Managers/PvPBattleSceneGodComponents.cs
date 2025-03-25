using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Music;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodComponents : MonoBehaviour, IPvPBattleSceneGodComponents
    {
        public AudioSource prioritisedSoundPlayerAudioSource;
        public IAudioSource PrioritisedSoundPlayerAudioSource { get; private set; }

        public AudioSource uiSoundsAudioSource;
        public IAudioSource UISoundsAudioSource { get; private set; }

        public LayeredMusicPlayerInitialiser musicPlayerInitialiser;
        public LayeredMusicPlayerInitialiser MusicPlayerInitialiser => musicPlayerInitialiser;

        public WindInitialiser windInitialiser;
        public WindInitialiser WindInitialiser => windInitialiser;

        public PvPCloudInitialiser cloudInitialiser;
        public PvPCloudInitialiser CloudInitialiser => cloudInitialiser;

        public PvPSkyboxInitialiser SkyboxInitialiser { get; private set; }
        public PvPClickableEmitter backgroundClickableEmitter;
        public IClickableEmitter BackgroundClickableEmitter => backgroundClickableEmitter;

        public TargetIndicatorController targetIndicator;
        public ITargetIndicator TargetIndicator => targetIndicator;

        public PvPHotkeyInitialiser hotkeyInitialiser;
        public PvPHotkeyInitialiser HotkeyInitialiser => hotkeyInitialiser;

        public IDeferrer Deferrer { get; private set; }
        public IDeferrer RealTimeDeferrer { get; private set; }

        public LifetimeEventBroadcaster LifetimeEvents { get; private set; }

        private UpdaterProvider _updaterProvider;
        public IUpdaterProvider UpdaterProvider => _updaterProvider;

        //        public void Initialise_Client(SettingsManager settingsManager)
        public void Initialise(SettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(
                backgroundClickableEmitter,
                targetIndicator,
                prioritisedSoundPlayerAudioSource,
                uiSoundsAudioSource,
                musicPlayerInitialiser,
                windInitialiser,
                cloudInitialiser,
                hotkeyInitialiser);
            Assert.IsNotNull(settingsManager);

            PrioritisedSoundPlayerAudioSource
                = new EffectVolumeAudioSource(
                    new AudioSourceBC(prioritisedSoundPlayerAudioSource),
                    settingsManager, 0);
            UISoundsAudioSource
                = new EffectVolumeAudioSource(
                    new AudioSourceBC(uiSoundsAudioSource),
                    settingsManager, 1);

            SkyboxInitialiser = GetComponent<PvPSkyboxInitialiser>();
            Assert.IsNotNull(SkyboxInitialiser);

            LifetimeEventBroadcaster lifetimeEvents = GetComponent<LifetimeEventBroadcaster>();
            Assert.IsNotNull(lifetimeEvents);
            LifetimeEvents = lifetimeEvents;
            targetIndicator.Initialise();

            Deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(Deferrer);

            RealTimeDeferrer = GetComponent<RealTimeDeferrer>();
            Assert.IsNotNull(RealTimeDeferrer);

            _updaterProvider = GetComponentInChildren<UpdaterProvider>();
            Assert.IsNotNull(_updaterProvider);
            _updaterProvider.Initialise();
        }


        /*        public void Initialise_Server()
                {
                    PvPHelper.AssertIsNotNull(
                        backgroundClickableEmitter,
                        targetIndicator,
                        prioritisedSoundPlayerAudioSource,
                        uiSoundsAudioSource,
                        musicPlayerInitialiser,
                        windInitialiser,
                        cloudInitialiser,
                        hotkeyInitialiser);

                    SkyboxInitialiser = GetComponent<PvPSkyboxInitialiser>();
                    Assert.IsNotNull(SkyboxInitialiser);

                    PvPLifetimeEventBroadcaster lifetimeEvents = GetComponent<PvPLifetimeEventBroadcaster>();
                    Assert.IsNotNull(lifetimeEvents);
                    LifetimeEvents = lifetimeEvents;
                    targetIndicator.Initialise();

                    Deferrer = GetComponent<PvPTimeScaleDeferrer>();
                    Assert.IsNotNull(Deferrer);

                    RealTimeDeferrer = GetComponent<PvPRealTimeDeferrer>();
                    Assert.IsNotNull(RealTimeDeferrer);

                    _updaterProvider = GetComponentInChildren<PvPUpdaterProvider>();
                    Assert.IsNotNull(_updaterProvider);
                    _updaterProvider.Initialise();
                }*/
    }
}
