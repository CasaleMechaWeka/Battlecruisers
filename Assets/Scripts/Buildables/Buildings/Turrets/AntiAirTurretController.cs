using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class AntiAirTurretController : DefensiveTurret
	{
		protected override void OnInitialised()
		{
			base.OnInitialised();
			_attackCapabilities.Add(TargetType.Aircraft);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateLeadingAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}

