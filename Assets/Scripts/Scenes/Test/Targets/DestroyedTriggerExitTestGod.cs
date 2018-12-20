using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetDetectors;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Targets
{
    public class DestroyedTriggerExitTestGod : MonoBehaviour
    {
        private AttackBoatController _ship;
        public CircleTargetDetectorController _detector;

        void Start()
        {
            Helper helper = new Helper();

            // Setup target attack boats
            _ship = FindObjectOfType<AttackBoatController>();
            helper.InitialiseUnit(_ship, Faction.Reds);
            _ship.StartConstruction();

            // Setup target detector
            //CircleTargetDetector[] detectors = FindObjectsOfType<CircleTargetDetector>();
            //foreach (CircleTargetDetectorController detector in detectors)
            {
                _detector.Initialise(radiusInM: 3.5f);
                _detector.StartDetecting();
            }

            // Destroy ship in 1 second
            //float timeInS = 1;
            //Invoke("DestroyShip", timeInS);
        }

        private void DestroyShip()
        {
            _ship.TakeDamage(_ship.MaxHealth, damageSource: null);
        }
    }
}
