using BattleCruisers.Utils.BattleScene;
using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public interface IMarker : IDestructable
    {
        bool IsVisible { set; }
        Vector2 OnScreenPostion { set; }
    }
}