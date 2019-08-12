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
            IRandomGenerator random = new RandomGenerator();

            foreach (AdvancedExplosion explosion in explosions)
            {
                explosion.Initialise(random);
                explosion.Activate(explosion.transform.position);
            }
        }
    }
}