using BattleCruisers.Effects.Explosions;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class BasicExplosionTestGod : MonoBehaviour
    {
        void Start()
        {
            Explosion explosion = FindObjectOfType<Explosion>();
            explosion.Initialise(radiusInM: 2, durationInS: 1);
            explosion.Show(explosion.transform.position);
        }
    }
}
