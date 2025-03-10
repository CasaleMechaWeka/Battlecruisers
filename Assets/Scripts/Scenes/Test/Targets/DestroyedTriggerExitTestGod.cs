using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetDetectors;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Targets
{
    public class DestroyedTriggerExitTestGod : TestGodBase
    {
        private AttackBoatController _ship;
        public CircleTargetDetectorController _detector;

        protected override List<GameObject> GetGameObjects()
        {
            _ship = FindObjectOfType<AttackBoatController>();

            return new List<GameObject>()
            {
                _ship.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup target attack boats
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
