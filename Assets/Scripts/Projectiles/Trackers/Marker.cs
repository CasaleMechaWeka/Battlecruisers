using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public class Marker : MonoBehaviour, IMarker
    {
        public bool IsVisible { set { gameObject.SetActive(value); } }
        public Vector2 OnScreenPostion { set { transform.position = value; } }

        public void RemoveFromScene()
        {
            Destroy(gameObject);
        }
    }
}