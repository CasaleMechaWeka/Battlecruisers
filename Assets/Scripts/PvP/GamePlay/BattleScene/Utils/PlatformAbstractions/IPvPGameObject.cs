using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public interface IPvPGameObject
    {
        bool IsVisible { get; set; }
        Vector3 Position { get; set; }
    }
}
