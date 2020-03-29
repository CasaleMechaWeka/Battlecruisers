using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    // FELIX  Should be able to remove once all projectiles have their own implementation?
    public class DummyProjectileTrail : MonoBehaviour, IProjectileTrail
    {
        public void Initialise() { }
        public void ShowAllEffects() { }
        public void HideAliveEffects() { }
        public void HideAllEffects() { }
    }
}