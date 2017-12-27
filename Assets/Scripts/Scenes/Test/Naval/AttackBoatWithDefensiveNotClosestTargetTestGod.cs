using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    // FELIX  Avoid duplicate code with:  Frigate vs Defensive not close enough, & then AB vs Elevated target
    public class AttackBoatWithDefensiveNotClosestTargetTestGod : MonoBehaviour 
	{
		void Start()
		{
			Helper helper = new Helper();

            // Turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
                turret.StartConstruction();
			}

            // Fake cruiser
            IBuilding blockingCruiserImitiation = FindObjectOfType<NavalFactory>();
            helper.InitialiseBuilding(blockingCruiserImitiation, Faction.Reds);
            blockingCruiserImitiation.StartConstruction();

            // Attack boat
            AttackBoatController boat = FindObjectOfType<AttackBoatController>();
            helper.InitialiseUnit(boat, Faction.Blues);
			boat.StartConstruction();
		}
	}
}
