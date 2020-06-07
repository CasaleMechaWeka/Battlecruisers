using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSceneGodComponents : MonoBehaviour, IBattleSceneGodComponents
    {
        public IDeferrer Deferrer { get; private set; }

        [SerializeField]
        private AudioSource prioritisedSoundPlayerAudioSource;
        public IAudioSource PrioritisedSoundPlayerAudioSource { get; private set; }

        [SerializeField]
        private AudioSource uiSoundsAudioSource;
        public IAudioSource UISoundsAudioSource { get; private set; }

        public CloudInitialiser CloudInitialiser { get; private set; }
        public SkyboxInitialiser SkyboxInitialiser { get; private set; }
        public LayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; private set; }
        public ILifetimeEventBroadcaster LifetimeEvents { get; private set; }

        private UpdaterProvider _updaterProvider;
        public IUpdaterProvider UpdaterProvider => _updaterProvider;

        public ClickableEmitter backgroundClickableEmitter;
        public IClickableEmitter BackgroundClickableEmitter => backgroundClickableEmitter;

        public AudioListener audioListener;
        public IGameObject AudioListener { get; private set; }

        public TargetIndicatorController targetIndicator;
        public ITargetIndicator TargetIndicator => targetIndicator;

        public void Initialise()
        {
            Helper.AssertIsNotNull(backgroundClickableEmitter, audioListener, targetIndicator, prioritisedSoundPlayerAudioSource, uiSoundsAudioSource);

            AudioListener = new GameObjectBC(audioListener.gameObject);

            Deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(Deferrer);

            PrioritisedSoundPlayerAudioSource = new AudioSourceBC(prioritisedSoundPlayerAudioSource);
            UISoundsAudioSource = new AudioSourceBC(uiSoundsAudioSource);

            CloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(CloudInitialiser);

            SkyboxInitialiser = GetComponent<SkyboxInitialiser>();
            Assert.IsNotNull(SkyboxInitialiser);

            MusicPlayerInitialiser = GetComponentInChildren<LayeredMusicPlayerInitialiser>();
            Assert.IsNotNull(MusicPlayerInitialiser);

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