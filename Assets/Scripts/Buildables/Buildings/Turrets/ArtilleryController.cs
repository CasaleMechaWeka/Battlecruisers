using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class ArtilleryController : OffensiveTurret
	{
		protected override void OnAwake()
		{
			base.OnAwake();
			_attackCapabilities.Add(TargetType.Buildings);
			_attackCapabilities.Add(TargetType.Cruiser);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateArtilleryAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
