using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
    public class WaterImpactHandler : MonoBehaviour
    {
        public ParticleSystem splashPrefab;

        private void Awake()
        {
            Assert.IsNotNull(splashPrefab);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);

            Quaternion rotation = Quaternion.Euler(x: -90, y: 0, z: 0);
            ParticleSystem splash = Instantiate(splashPrefab, collision.transform.position, rotation);
            splash.Play();
        }
    }
}