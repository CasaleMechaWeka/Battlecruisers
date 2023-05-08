using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails
{
    public class PvPMissileTrailController : MonoBehaviour, IPvPProjectileTrail
    {
        public SpriteRenderer glow, missileFlare;
        public TrailRenderer trail;

        public void Initialise()
        {
            PvPHelper.AssertIsNotNull(glow, missileFlare, trail);
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