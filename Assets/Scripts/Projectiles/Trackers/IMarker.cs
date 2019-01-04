using BattleCruisers.Utils.BattleScene;
using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public interface IMarker : IRemovable
    {
        bool IsVisible { set; }
        Vector2 OnScreenPostion { set; }
    }
}