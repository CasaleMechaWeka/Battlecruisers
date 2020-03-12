using UnityEngine;

namespace BattleCruisers.Cruisers.Fog
{
    public enum FogStrength
    {
        Weak, Strong
    }

    public class FogOfWar : MonoBehaviourWrapper
    {
        private const float STRONG_FOG_ALPHA = 1;
        private const float WEAK_FOG_ALPHA = 0.2f;

        public void Initialise(FogStrength fogStrength)
        {
            IsVisible = false;

            float fogAlpha = fogStrength == FogStrength.Weak ? WEAK_FOG_ALPHA : STRONG_FOG_ALPHA;
            // Black
            Color fogColor = new Color(r: 0, g: 0, b: 0, a: fogAlpha);

            SpriteRenderer[] clouds = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer cloud in clouds)
            {
                cloud.color = fogColor;
            }
        }
    }
}
