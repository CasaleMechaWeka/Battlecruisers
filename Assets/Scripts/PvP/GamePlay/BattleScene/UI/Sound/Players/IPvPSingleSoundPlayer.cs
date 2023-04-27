using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public interface IPvPSingleSoundPlayer
    {
        bool IsPlayingSound { get; }

        Task<AsyncOperationHandle<AudioClip>> PlaySoundAsync(IPvPSoundKey soundKey, bool loop = false);
        void PlaySound(IPvPAudioClipWrapper sound, bool loop = false);
        void Stop();
    }
}
