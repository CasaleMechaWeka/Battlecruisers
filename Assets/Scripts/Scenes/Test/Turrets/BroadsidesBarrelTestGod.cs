using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BroadsidesBarrelTestGod : MonoBehaviour
	{
		void Start()
		{
            // Initialise target
            Helper helper = new Helper();
            Factory target = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(target);


            // Initialise double barrel
            BarrelController doubleBarrel = FindObjectOfType<BarrelController>();
			doubleBarrel.StaticInitialise();
			doubleBarrel.Target = target;

            IBarrelControllerArgs barrelControllerArgs
                = helper.CreateBarrelControllerArgs(
                    doubleBarrel,
                    targetFilter: new ExactMatchTargetFilter() { Target = target },
                    angleCalculator: new ArtilleryAngleCalculator(new AngleHelper(), doubleBarrel.ProjectileStats));

            doubleBarrel.Initialise(barrelControllerArgs);
		}
	}
}
