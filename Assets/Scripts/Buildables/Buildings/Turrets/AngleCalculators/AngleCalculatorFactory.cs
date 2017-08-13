using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculatorFactory : IAngleCalculatorFactory
	{
		public IAngleCalculator CreateAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new AngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateArtilleryAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new ArtilleryAngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateMortarAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MortarAngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateLeadingAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new LeadingAngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateStaticAngleCalculator(ITargetPositionPredictorFactory targetPositionPredictorFactory, float desiredAngleInDegrees)
		{
			return new StaticAngleCalculator(targetPositionPredictorFactory, desiredAngleInDegrees);
		}
	}
}

