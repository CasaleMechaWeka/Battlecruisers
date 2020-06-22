using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Scenes.Test.Utilities.Sound;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class SoundBalancingTestGod : NavigationTestGod
    {
        private MusicController _music;
        private AudioListener _audioListener;

        public WindButtonsPanelController windButtonsPanelController;
        public AudioSource audioSource;

        protected override void Setup(Utilities.Helper helper)
        {
            base.Setup(helper);
            BCUtils.Helper.AssertIsNotNull(windButtonsPanelController, audioSource);

            _audioListener = Camera.main.GetComponentInChildren<AudioListener>();
            Assert.IsNotNull(_audioListener);

            _music = FindObjectOfType<MusicController>();
            Assert.IsNotNull(_music);
            _music.Initialise();

            AudioSource singleSoundPlayerSource = transform.FindNamedComponent<AudioSource>("SingleSoundPlayer");
            BuildableInitialisationArgs initialisationArgs = helper.CreateBuildableInitialisationArgs();
            SetupSoundPlayerObjects(singleSoundPlayerSource, initialisationArgs.FactoryProvider.PoolProviders);

            windButtonsPanelController.Initialise(_camera, _cameraCalculatorSettings);
        }

        private void SetupSoundPlayerObjects(AudioSource singleSoundPlayerSource, IPoolProviders poolProviders)
        {
            SoundFetcher soundFetcher = new SoundFetcher();

            ISoundPlayer soundPlayer
                = new SoundPlayer(
                    soundFetcher,
                    poolProviders.AudioSourcePool);

            ISingleSoundPlayer singleSoundPlayer
                = new SingleSoundPlayer(
                    soundFetcher,
                    new AudioSourceBC(singleSoundPlayerSource));

            SoundGroupController[] soundGroups = FindObjectsOfType<SoundGroupController>();

            foreach (SoundGroupController group in soundGroups)
            {
                group.Initialise(soundPlayer, singleSoundPlayer);
            }
        }
    }
}