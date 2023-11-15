using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public interface IPvPInput
    {
        Vector3 MousePosition { get; }
        Vector2 MouseScrollDelta { get; }
        int TouchCount { get; }

        Vector2 GetTouchPosition(int touchIndex);
        bool GetKeyUp(KeyCode key);
        KeyCode GetFirstKeyDown();
    }
}
