using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class ExplosionSizeTestGod : MonoBehaviour
    {
        void Start()
        {
            AdvancedExplosion[] explosions = GetComponentsInChildren<AdvancedExplosion>();

            foreach (AdvancedExplosion explosion in explosions)
            {
                explosion.Initialise(RandomGenerator.Instance);
                explosion.Activate(explosion.transform.position);
            }
        }
    }
}