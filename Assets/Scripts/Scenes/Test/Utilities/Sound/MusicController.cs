using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class MusicController : MonoBehaviour
    {
        private LayeredMusicPlayerInitialiser _musicInitialiser;
        private ILayeredMusicPlayer _musicPlayer;
        private ISoundFetcher _soundFetcher;
        private ICircularList<SoundKeyPair> _songs;

        // FELIX  Display music name

        [Tooltip("0-5")]
        public int startingIndex = 3;

        public async void Initialise()
        {
            IList<SoundKeyPair> songs = new List<SoundKeyPair>()
            {
                SoundKeys.Music.Background.Bobby,
                SoundKeys.Music.Background.Confusion,
                SoundKeys.Music.Background.Experimental,
                SoundKeys.Music.Background.Juggernaut,
                SoundKeys.Music.Background.Nothing,
                SoundKeys.Music.Background.Sleeper
            };
            _songs = new CircularList<SoundKeyPair>(songs);
            _songs.Index = startingIndex;

            _soundFetcher = new SoundFetcher();
            _musicInitialiser = GetComponentInChildren<LayeredMusicPlayerInitialiser>();
            Assert.IsNotNull(_musicInitialiser);
            _musicPlayer = await CreateMusicPlayer();
        }

        private async Task<ILayeredMusicPlayer> CreateMusicPlayer()
        {
            return
                await _musicInitialiser.CreatePlayerAsync(
                    _soundFetcher,
                    _songs.Current());
        }

        public void PlayBackground()
        {
            _musicPlayer.Play();
        }

        public void PlayDanger()
        {
            _musicPlayer.PlaySecondary();
        }

        public void Stop()
        {
            _musicPlayer.Stop();
        }

        public void StopDanger()
        {
            _musicPlayer.StopSecondary();
        }

        public async void Next()
        {
            Stop();
            _musicPlayer = await CreateMusicPlayer();
        }
    }
}