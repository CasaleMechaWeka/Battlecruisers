using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public interface IPvPSingleSoundPlayer
    {
        bool IsPlayingSound { get; }

        Task<AsyncOperationHandle<AudioClip>> PlaySoundAsync(ISoundKey soundKey, bool loop = false);
        void PlaySound(IAudioClipWrapper sound, bool loop = false);
        void Stop();
    }
}
