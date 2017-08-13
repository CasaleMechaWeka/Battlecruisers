using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretTestsGod : MonoBehaviour 
	{
		void Start () 
		{
			Helper helper = new Helper();

            AttackBoatController boat = FindObjectOfType<AttackBoatController>();
			helper.InitialiseBuildable(boat, Faction.Blues);
			boat.StartConstruction();

            TurretController turret = FindObjectOfType<TurretController>();
			helper.InitialiseBuildable(turret, Faction.Reds);
			turret.StartConstruction();
		}
	}
}
