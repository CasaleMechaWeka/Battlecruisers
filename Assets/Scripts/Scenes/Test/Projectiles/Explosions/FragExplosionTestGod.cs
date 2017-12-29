using BattleCruisers.Projectiles.Explosions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Projectiles.Explosions
{
    public class FragExplosionTestGod : MonoBehaviour
    {
        void Start()
        {
            Explosion[] explosions = FindObjectsOfType<Explosion>();

            //Assert.AreEqual(2, explosions.Length);

            Explosion smallExplosion = explosions[0];
            smallExplosion.Initialise(radiusInM: 1, durationInS: 999);
            smallExplosion.Show(new Vector3(-2, 0));

            //Explosion bigExplosion = explosions[0];
            //bigExplosion.Initialise(radiusInM: 2, durationInS: 999);
            //bigExplosion.Show(new Vector3(2, 0));
        }
    }
}
