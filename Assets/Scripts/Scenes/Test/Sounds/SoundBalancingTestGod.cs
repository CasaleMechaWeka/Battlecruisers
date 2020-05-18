using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities.Sound;
using BattleCruisers.UI.Music;
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
        private ILayeredMusicPlayer _musicPlayer;
        private AudioListener _audioListener;

        protected async override void Setup(Utilities.Helper helper)
        {
            base.Setup(helper);

            _audioListener = Camera.main.GetComponent<AudioListener>();
            Assert.IsNotNull(_audioListener);

            LayeredMusicPlayerInitialiser musicInitialiser = GetComponentInChildren<LayeredMusicPlayerInitialiser>();
            Assert.IsNotNull(musicInitialiser);
            _musicPlayer
                = await musicInitialiser.CreatePlayerAsync(
                    new SoundFetcher(),
                    SoundKeys.Music.Background.Juggernaut);

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

            SoundPlayerController[] soundPlayerObjects = FindObjectsOfType<SoundPlayerController>();

            foreach (SoundPlayerController soundPlayerObject in soundPlayerObjects)
            {
                soundPlayerObject.Initialise(soundPlayer);
            }

            SoundGroupController[] soundGroups = FindObjectsOfType<SoundGroupController>();

            foreach (SoundGroupController group in soundGroups)
            {
                group.Initialise(soundPlayer, singleSoundPlayer);
            }
        }

        public void PlayBackgroundMusic()
        {
            _musicPlayer.Play();
        }

        public void PlayDangerMusic()
        {
            _musicPlayer.PlaySecondary();
        }

        public void StopMusic()
        {
            _musicPlayer.Stop();
        }

        protected override void Update()
        {
            base.Update();

            if (_audioListener != null)
            {
                //Debug.Log($"Audio listener position: {_audioListener.transform.position}");
            }
        }
    }
}