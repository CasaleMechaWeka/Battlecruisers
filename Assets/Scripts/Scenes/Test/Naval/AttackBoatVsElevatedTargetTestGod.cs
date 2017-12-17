using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatVsElevatedTargetTestGod : MonoBehaviour 
	{
		void Start()
		{
			Helper helper = new Helper();

            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Reds);
            turret.StartConstruction();

            IBuilding blockingCruiserImitiation = FindObjectOfType<NavalFactory>();
            helper.InitialiseBuilding(blockingCruiserImitiation, Faction.Reds);
            blockingCruiserImitiation.StartConstruction();

            AttackBoatController boat = FindObjectOfType<AttackBoatController>();
			ITargetsFactory targetsFactory = helper.CreateTargetsFactory(globalTarget: turret.GameObject);
            helper.InitialiseUnit(boat, Faction.Blues, targetsFactory: targetsFactory);
			boat.StartConstruction();
		}
	}
}
