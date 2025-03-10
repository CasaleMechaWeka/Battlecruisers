using BattleCruisers.Data.Settings;
using BattleCruisers.Hotkeys;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGodComponents : MonoBehaviour, IBattleSceneGodComponents
    {
        public IDeferrer Deferrer { get; private set; }
        public IDeferrer RealTimeDeferrer { get; private set; }

        public AudioSource prioritisedSoundPlayerAudioSource;
        public IAudioSource PrioritisedSoundPlayerAudioSource { get; private set; }

        public AudioSource uiSoundsAudioSource;
        public IAudioSource UISoundsAudioSource { get; private set; }

        public LayeredMusicPlayerInitialiser musicPlayerInitialiser;
        public LayeredMusicPlayerInitialiser MusicPlayerInitialiser => musicPlayerInitialiser;

        public WindInitialiser windInitialiser;
        public WindInitialiser WindInitialiser => windInitialiser;

        public CloudInitialiser cloudInitialiser;
        public CloudInitialiser CloudInitialiser => cloudInitialiser;

        public SkyboxInitialiser SkyboxInitialiser { get; private set; }
        public ILifetimeEventBroadcaster LifetimeEvents { get; private set; }

        private UpdaterProvider _updaterProvider;
        public IUpdaterProvider UpdaterProvider => _updaterProvider;

        public ClickableEmitter backgroundClickableEmitter;
        public IClickableEmitter BackgroundClickableEmitter => backgroundClickableEmitter;

        public TargetIndicatorController targetIndicator;
        public ITargetIndicator TargetIndicator => targetIndicator;

        public HotkeyInitialiser hotkeyInitialiser;
        public HotkeyInitialiser HotkeyInitialiser => hotkeyInitialiser;

        public void Initialise(ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(
                backgroundClickableEmitter, 
                targetIndicator, 
                prioritisedSoundPlayerAudioSource, 
                uiSoundsAudioSource, 
                musicPlayerInitialiser, 
                windInitialiser,
                cloudInitialiser,
                hotkeyInitialiser);
            Assert.IsNotNull(settingsManager);

            Deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(Deferrer);

            RealTimeDeferrer = GetComponent<RealTimeDeferrer>();
            Assert.IsNotNull(RealTimeDeferrer);

            PrioritisedSoundPlayerAudioSource
                = new EffectVolumeAudioSource(
                    new AudioSourceBC(prioritisedSoundPlayerAudioSource),
                    settingsManager, 0);
            UISoundsAudioSource
                = new EffectVolumeAudioSource(
                    new AudioSourceBC(uiSoundsAudioSource),
                    settingsManager, 1);

            SkyboxInitialiser = GetComponent<SkyboxInitialiser>();
            Assert.IsNotNull(SkyboxInitialiser);

            _updaterProvider = GetComponentInChildren<UpdaterProvider>();
            Assert.IsNotNull(_updaterProvider);
            _updaterProvider.Initialise();

            LifetimeEventBroadcaster lifetimeEvents = GetComponent<LifetimeEventBroadcaster>();
            Assert.IsNotNull(lifetimeEvents);
            LifetimeEvents = lifetimeEvents;

            targetIndicator.Initialise();
        }
    }
}