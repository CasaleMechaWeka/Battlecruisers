using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
            throw new NotImplementedException();
        }
    }
}