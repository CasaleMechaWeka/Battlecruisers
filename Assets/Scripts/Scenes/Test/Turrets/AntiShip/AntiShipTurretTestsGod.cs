using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretTestsGod : MonoBehaviour 
	{
		public AttackBoatController boat;
		public TurretController rightTurret;

		void Start () 
		{
			Helper helper = new Helper();

			helper.InitialiseBuildable(boat, Faction.Blues);
			boat.StartConstruction();

			helper.InitialiseBuildable(rightTurret, Faction.Reds);
			rightTurret.StartConstruction();
		}
	}
}
