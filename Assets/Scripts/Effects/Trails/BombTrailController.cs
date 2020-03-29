using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    public class BombTrailController : MonoBehaviour, IProjectileTrail
    {
        public SpriteRenderer glow, bullet;
        public TrailRenderer trail;

        public void Initialise()
        {
            Helper.AssertIsNotNull(glow, bullet, trail);
        }

        public void ShowAllEffects()
        {
            glow.enabled = true;
            bullet.enabled = true;
            trail.Clear();
        }

        public void HideEffects()
        {
            glow.enabled = false;
            bullet.enabled = false;
        }
    }
}