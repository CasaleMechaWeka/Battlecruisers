using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio
{
    public interface IPvPAudioClipWrapper
    {
        AudioClip AudioClip { get; }
        float Length { get; }
        AsyncOperationHandle<AudioClip> Handle { get; }
    }

}

