using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public interface IPvPPointerEventData
    {
        Vector2 Delta { get; }
        Vector2 Position { get; }
    }
}