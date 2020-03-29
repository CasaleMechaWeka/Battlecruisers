using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    public class MissileTrailController : MonoBehaviour, IProjectileTrail
    {
        public SpriteRenderer glow, missileFlare;
        public TrailRenderer trail;

        public void Initialise()
        {
            Helper.AssertIsNotNull(glow, missileFlare, trail);
        }

        public void ShowAllEffects()
        {
            glow.enabled = true;
            missileFlare.enabled = true;
            trail.Clear();
        }

        public void HideEffects()
        {
            glow.enabled = false;
            missileFlare.enabled = false;
        }
    }
}