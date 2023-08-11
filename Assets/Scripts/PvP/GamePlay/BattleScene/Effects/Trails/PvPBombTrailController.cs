using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails
{
    public class PvPBombTrailController : MonoBehaviour, IPvPProjectileTrail
    {
        public SpriteRenderer glow, bullet;
        public TrailRenderer trail;

        public void Initialise()
        {
            PvPHelper.AssertIsNotNull(glow, bullet, trail);
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
        public void SetVisibleTrail(bool isVisible)
        {
            trail.enabled = isVisible;
        }
    }
}