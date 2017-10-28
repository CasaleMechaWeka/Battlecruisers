using BattleCruisers.Projectiles.Explosions;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ExplosionTestGod : MonoBehaviour
    {
        void Start()
        {
            Explosion explosion = FindObjectOfType<Explosion>();
            explosion.Initialise(radiusInM: 2, durationInS: 1);
            explosion.Show(explosion.transform.position);
        }
    }
}
