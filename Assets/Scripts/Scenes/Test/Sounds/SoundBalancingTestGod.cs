using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Scenes.Test.Utilities.Sound;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class SoundBalancingTestGod : NavigationTestGod
    {
        private MusicController _music;
        private AudioListener _audioListener;

        protected override void Setup(Utilities.Helper helper)
        {
            base.Setup(helper);

            _audioListener = Camera.main.GetComponentInChildren<AudioListener>();
            Assert.IsNotNull(_audioListener);

            _music = FindObjectOfType<MusicController>();
            Assert.IsNotNull(_music);
            _music.Initialise();

            AudioSource singleSoundPlayerSource = transform.FindNamedComponent<AudioSource>("SingleSoundPlayer");
            SetupSoundPlayerObjects(singleSoundPlayerSource);
        }

        private void SetupSoundPlayerObjects(AudioSource singleSoundPlayerSource)
        {
            SoundFetcher soundFetcher = new SoundFetcher();

            ISoundPlayer soundPlayer
                = new SoundPlayer(
                    soundFetcher,
                    new AudioClipPlayer(),
                    new GameObjectBC(_audioListener.gameObject));

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