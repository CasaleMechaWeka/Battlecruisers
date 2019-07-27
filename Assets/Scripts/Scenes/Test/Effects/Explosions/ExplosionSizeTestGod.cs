using BattleCruisers.Effects.Explosions;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class ExplosionSizeTestGod : MonoBehaviour
    {
        void Start()
        {
            AdvancedExplosion[] explosions = GetComponentsInChildren<AdvancedExplosion>(includeInactive: true);

            foreach (AdvancedExplosion explosion in explosions)
            {
                explosion.Initialise();
                explosion.gameObject.SetActive(true);
            }
        }
    }
}