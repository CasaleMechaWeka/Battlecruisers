using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class LocalBoosterTestGod : MonoBehaviour
    {
        void Start()
        {
            Helper helper = new Helper();


            // Setup target
            AirFactory target = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(target, Faction.Reds);
            target.StartConstruction();
			
			
			// Setup artillery slot
			ICruiser parentCruiser = Substitute.For<ICruiser>();
			Slot slotToBoost = FindObjectOfType<Slot>();
			slotToBoost.Initialise(parentCruiser);


            // Setup artillery
            IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter();
            targetFilter.Target = target;
            ITargetsFactory targetsFactory = helper.CreateTargetsFactory(target.GameObject, targetFilter);

            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Blues, targetsFactory: targetsFactory, localBoostProviders: slotToBoost.BoostProviders);
            turret.StartConstruction();


            // Setup local booster
            LocalBoosterController localBooster = FindObjectOfType<LocalBoosterController>();
            helper.InitialiseBuilding(localBooster);
            localBooster.StartConstruction();
        }
    }
}
