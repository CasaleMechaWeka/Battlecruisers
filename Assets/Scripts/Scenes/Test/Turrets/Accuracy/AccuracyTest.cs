using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.Accuracy
{
    public class AccuracyTest : MonoBehaviour, ITestScenario
    {
        public Camera Camera { get; private set; }

        public void Initialise()
        {
            // Show accuracy
            ITurretStats stats = GetComponentInChildren<ITurretStats>();
            TextMesh accuracyText = GetComponentInChildren<TextMesh>();
            accuracyText.text = stats.Accuracy * 100 + "%";


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


            Helper helper = new Helper();


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

            ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);
            IAccuracyAdjusterFactory accuracyAdusterFactory = new AccuracyAdjusterFactory();

            helper
                .InitialiseBuilding(
                    turret, 
                    Faction.Reds, 
                    targetsFactory: targetsFactory,
                    accuracyAdjusterFactory: accuracyAdusterFactory);

            turret.StartConstruction();
        }
    }
}
