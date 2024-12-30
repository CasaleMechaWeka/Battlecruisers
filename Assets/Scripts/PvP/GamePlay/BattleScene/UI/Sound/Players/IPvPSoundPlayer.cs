using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public interface IPvPSoundPlayer
    {
        Task PlaySoundAsync(ISoundKey soundKey, Vector2 position);
        void PlaySound(IAudioClipWrapper sound, Vector2 position);
    }
}
