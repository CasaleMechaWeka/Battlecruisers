using BattleCruisers.UI;
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
        public IAudioSource AudioSource { get; private set; }
        public CloudInitialiser CloudInitialiser { get; private set; }
        public SkyboxInitialiser SkyboxInitialiser { get; private set; }
        public LayeredMusicPlayerInitialiser MusicPlayerInitialiser { get; private set; }
        public ILifetimeEventBroadcaster LifetimeEvents { get; private set; }

        private UpdaterProvider _updaterProvider;
        public IUpdaterProvider UpdaterProvider => _updaterProvider;

        public ClickableEmitter backgroundClickableEmitter;
        public IClickableEmitter BackgroundClickableEmitter => backgroundClickableEmitter;

        public GameObject audioListener;
        public IGameObject AudioListener { get; private set; }

        public void Initialise()
        {
            Helper.AssertIsNotNull(backgroundClickableEmitter, audioListener);

            AudioListener = new GameObjectBC(audioListener);

            Deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(Deferrer);

            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            AudioSource = new AudioSourceBC(platformAudioSource);

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
        }
    }
}