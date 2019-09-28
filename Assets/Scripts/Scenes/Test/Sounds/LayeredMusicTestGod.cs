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
                    SoundKeys.Music.Background.Sleeper);
                    //SoundKeys.Music.Background.Nothing);
                    //SoundKeys.Music.Background.Experimental);
                    //SoundKeys.Music.Background.Confusion);
                    //SoundKeys.Music.Background.Bobby);
                    //SoundKeys.Music.Background.Kentient);
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