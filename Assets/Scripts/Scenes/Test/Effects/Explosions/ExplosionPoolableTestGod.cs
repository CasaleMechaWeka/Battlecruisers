using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class ExplosionPoolableTestGod : MonoBehaviour
    {
        void Start()
        {
            AdvancedExplosion[] explosions = FindObjectsOfType<AdvancedExplosion>();
            IRandomGenerator random = new RandomGenerator();

            foreach (AdvancedExplosion explosion in explosions)
            {
                explosion.Initialise(random);
                explosion.Activate(explosion.transform.position);
                explosion.Deactivated += (sender, e) => explosion.Activate(explosion.transform.position);
            }
        }
    }
}