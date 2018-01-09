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
        void Start()
        {
            SoundFetcher fetcher = new SoundFetcher();

            ISoundKey mp3Key = SoundKeys.Firing.AntiAir;
            ISoundKey wavKey = SoundKeys.Engines.Archon;

            //IAudioClipWrapper audioClip = fetcher.GetSound(mp3Key);
            IAudioClipWrapper audioClip = fetcher.GetSound(wavKey);

            //AudioSource.PlayClipAtPoint(audioClip.AudioClip, default(Vector3));

            AudioSource source = GetComponent<AudioSource>();
            source.clip = audioClip.AudioClip;
            source.loop = true;
            source.Play();
        }
    }
}
