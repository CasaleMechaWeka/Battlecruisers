using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class LeadingAngleCalculator : AngleCalculator
	{
		protected override bool LeadsTarget { get { return true; } }

		public LeadingAngleCalculator(ITargetPositionPredictorFactory targetPositionPredictorFactory)
			: base(targetPositionPredictorFactory) 
		{ 
			_targetPositionPredictor = _targetPositionPredictorFactory.CreateLinearPredictor();
		}
	}
}
