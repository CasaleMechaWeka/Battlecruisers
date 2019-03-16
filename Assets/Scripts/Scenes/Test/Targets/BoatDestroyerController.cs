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
            Logging.Log(Tags.SHIPS, "BoatDestroyerController.Start()");
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Logging.Log(Tags.SHIPS, "BoatDestroyerController.OnTriggerEnter2D()");

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
                Logging.Log(Tags.SHIPS, "BoatDestroyerController.FixedUpdate()  Destroy ship :D");
                _ship.TakeDamage(_ship.MaxHealth, damageSource: null);
            }
        }
    }
}
