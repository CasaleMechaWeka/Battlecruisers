using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Sounds
{
    // FELIX  Delete this and the corresponding test scene (as soon as I have a better sound test scene :) )
    public class SoundsTestGod : MonoBehaviour
    {
        private AudioSource _source;

        void Start()
        {
            SoundFetcher fetcher = new SoundFetcher();

            ISoundKey mp3Key = SoundKeys.Firing.AntiAir;
            ISoundKey wavKey = SoundKeys.Engines.Archon;

            //IAudioClipWrapper audioClip = fetcher.GetSound(mp3Key);
            IAudioClipWrapper audioClip = fetcher.GetSound(wavKey);

            //AudioSource.PlayClipAtPoint(audioClip.AudioClip, default(Vector3));

            _source = GetComponent<AudioSource>();
            _source.clip = audioClip.AudioClip;
            _source.loop = true;
            _source.Play();
        }

        public void ToggleSound()
        {
            if (_source.isPlaying)
            {
                _source.Pause();
                //gameObject.SetActive(false);
            }
            else
            {
                //gameObject.SetActive(true);
                _source.UnPause();
            }
        }
    }
}
