using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Targets
{
    public class BoatDestroyerController : MonoBehaviour
    {
        private const bool FORCE_BUG = true;
        private AttackBoatController _ship;

        void Start()
        {
            Logging.LogMethod(Tags.SHIPS);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Logging.LogMethod(Tags.SHIPS);

            _ship = collider.gameObject.GetComponent<AttackBoatController>();
            Assert.IsNotNull(_ship);
            if (FORCE_BUG)
            {
                _ship.TakeDamage(_ship.MaxHealth, damageSource: null);
            }
        }

        void FixedUpdate()
        {
            if (_ship != null)
            {
                Logging.Log(Tags.SHIPS, "Destroy ship :D");
                _ship.TakeDamage(_ship.MaxHealth, damageSource: null);
            }
        }
    }
}
