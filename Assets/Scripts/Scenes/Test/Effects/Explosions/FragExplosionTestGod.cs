using BattleCruisers.Effects.Explosions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class FragExplosionTestGod : MonoBehaviour
    {
        void Start()
        {
            // Reduce game speed
            Time.timeScale = 0.1f;

            // Setup explosions
            Explosion[] explosions = FindObjectsOfType<Explosion>();

            Assert.AreEqual(2, explosions.Length);

            Explosion smallExplosion = explosions[0];
            smallExplosion.Initialise(radiusInM: 1, durationInS: 999);
            smallExplosion.Show(new Vector3(-2, 0));

            Explosion bigExplosion = explosions[1];
            bigExplosion.Initialise(radiusInM: 2, durationInS: 999);
            bigExplosion.Show(new Vector3(2, 0));
        }
    }
}
