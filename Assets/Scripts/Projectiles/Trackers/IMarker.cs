using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public interface IMarker
    {
        bool IsVisible { set; }
        Vector2 OnScreenPostion { set; }
    }
}