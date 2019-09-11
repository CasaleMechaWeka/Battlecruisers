using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class LayeredMusicTestGod : MonoBehaviour
    {
        private ILayeredMusicPlayer _musicPlayer;

        void Start()
        {
            LayeredMusicPlayerInitialiser initialiser = GetComponentInChildren<LayeredMusicPlayerInitialiser>();
            _musicPlayer
                = initialiser.CreatePlayer(
                    new SoundFetcher(),
                    SoundKeys.Music.Background.KentientBase,
                    SoundKeys.Music.Background.KentientDanger);
            _musicPlayer.Play();
        }

        public void StartDanger()
        {
            _musicPlayer.PlaySecondary();
        }

        public void StopDanger()
        {
            _musicPlayer.StopSecondary();
        }
    }
}