using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.UI.Sound.Players
{
    public class AudioClipPlayer : IAudioClipPlayer
    {
        public void PlaySound(IAudioClipWrapper soundClip, Vector3 position)
        {
            Logging.Log(Tags.SOUND, $"Sound: {soundClip.AudioClip}  Position: {position}  Camera position: {Camera.main.transform.position}");
            AudioSource.PlayClipAtPoint(soundClip.AudioClip, position);
        }
    }
}
