using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public interface IPvPSoundPlayer
    {
        Task PlaySoundAsync(IPvPSoundKey soundKey, Vector2 position);
        void PlaySound(IPvPAudioClipWrapper sound, Vector2 position);
    }
}
