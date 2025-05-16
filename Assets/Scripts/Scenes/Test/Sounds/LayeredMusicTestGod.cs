using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class LayeredMusicTestGod : MonoBehaviour
    {
        private LayeredMusicPlayer _musicPlayer;

        async void Start()
        {
            LayeredMusicPlayerInitialiser initialiser = GetComponentInChildren<LayeredMusicPlayerInitialiser>();
            _musicPlayer
                = await initialiser.CreatePlayerAsync(
                    //SoundKeys.Music.Background.Sleeper
                    //SoundKeys.Music.Background.Nothing
                    //SoundKeys.Music.Background.Experimental
                    //SoundKeys.Music.Background.Againagain
                    SoundKeys.Music.Background.Confusion,
                    //SoundKeys.Music.Background.Bobby
                    //SoundKeys.Music.Background.Juggernaut
                    DataProvider.SettingsManager);
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