using BattleCruisers.Projectiles.Explosions;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class ExplosionTestGod : MonoBehaviour
    {
        void Start()
        {
            Explosion explosion = FindObjectOfType<Explosion>();
            explosion.StaticInitailise();
            explosion.Show(radiusInM: 2, durationInS: 1);
        }
    }
}
