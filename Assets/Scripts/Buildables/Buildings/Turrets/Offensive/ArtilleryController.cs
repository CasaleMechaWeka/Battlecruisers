using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Offensive
{
	public class ArtilleryController : OffensiveTurret
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateArtilleryAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
