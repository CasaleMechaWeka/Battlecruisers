using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Trackers
{
    public class MarkerFactory : MonoBehaviour, IMarkerFactory
    {
        public Marker markerPrefab;

        // Seprate canvas to HUD to ensure HUD is above markers.
        public Canvas markerCanvas;

        public void Intialise()
        {
            Helper.AssertIsNotNull(markerPrefab, markerCanvas);
        }

        public IMarker CreateMarker()
        {
            return Instantiate(markerPrefab, markerCanvas.transform);
        }
    }
}