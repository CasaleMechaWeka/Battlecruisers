using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatVsElevatedTargetTestGod : ShipWithDefensiveNotClosestTargetTestGod 
	{
        protected override void InitialiseBoat(Helper helper, ShipController boat, TurretController[] turrets)
        {
            ITargetFactories targetFactories = helper.CreateTargetFactories(globalTarget: turrets[0].GameObject);
			helper.InitialiseUnit(boat, Faction.Blues, targetFactories: targetFactories);
        }
   	}
}
