using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldDetectionTestGod : TestGodBase
	{
		protected override void Start () 
		{
            base.Start();

			Helper helper = new Helper(updaterProvider: _updaterProvider);


			// Setup shields
			ShieldGenerator[] shields = FindObjectsOfType<ShieldGenerator>();
			Assert.IsTrue(shields.Length > 0);

			foreach (ShieldGenerator shield in shields)
			{
                helper.InitialiseBuilding(shield, Faction.Blues);
				shield.StartConstruction();
			}


			// Setup attack boats
			AttackBoatController[] attackBoats = FindObjectsOfType<AttackBoatController>();
			Assert.IsTrue(attackBoats.Length > 0);

			foreach (AttackBoatController attackBoat in attackBoats)
			{
                helper.InitialiseUnit(attackBoat, Faction.Reds, parentCruiserDirection: Direction.Left);
				attackBoat.StartConstruction();
			}
		}
	}
}
