using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretTestsGod : MonoBehaviour 
	{
		void Start()
		{
			Helper helper = new Helper();

            AttackBoatController boat = FindObjectOfType<AttackBoatController>();
            helper.InitialiseUnit(boat, Faction.Blues);
			boat.StartConstruction();

            TurretController turret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(turret, Faction.Reds);
			turret.StartConstruction();
		}
	}
}
