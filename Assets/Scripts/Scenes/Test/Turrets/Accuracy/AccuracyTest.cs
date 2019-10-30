using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets.Accuracy
{
    public class AccuracyTest : MonoBehaviour, ITestScenario
    {
        public Camera Camera { get; private set; }

        public void Initialise(Helper helper)
        {
            Assert.IsNotNull(helper);

            // Show accuracy
            ITurretStats stats = GetComponentInChildren<ITurretStats>();
            TextMesh accuracyText = GetComponentInChildren<TextMesh>();
            accuracyText.text = stats.Accuracy * 100 + "%";


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


            // Setup target
            AirFactory target = GetComponentInChildren<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Blues);
            target.StartConstruction();


            // Setup turret
            TurretController turret = GetComponentInChildren<TurretController>();
            ITargetFilter targetFilter = new ExactMatchTargetFilter()
            {
                Target = target
            };

            ITargetFactories targetFactories = helper.CreateTargetFactories(target.GameObject, targetFilter: targetFilter);
            IAccuracyAdjusterFactory accuracyAdusterFactory = new AccuracyAdjusterFactory();

            helper
                .InitialiseBuilding(
                    turret, 
                    Faction.Reds, 
                    targetFactories: targetFactories,
                    accuracyAdjusterFactory: accuracyAdusterFactory);

            turret.StartConstruction();
        }
    }
}
