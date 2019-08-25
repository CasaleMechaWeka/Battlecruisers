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

            foreach (AdvancedExplosion explosion in explosions)
            {
                explosion.Initialise(RandomGenerator.Instance);
                explosion.Activate(explosion.transform.position);
                explosion.Deactivated += (sender, e) => explosion.Activate(explosion.transform.position);
            }
        }
    }
}