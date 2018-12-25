using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    // FELIX  Show direction as well :D
    public class Marker : MonoBehaviour, IMarker
    {
        public bool IsVisible { set { gameObject.SetActive(value); } }
        public Vector2 OnScreenPostion { set { transform.position = value; } }
    }
}