namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
	{
		public int numOfDronesProvided;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }

		protected override void OnBuildableCompleted()
		{
			_droneManager.NumOfDrones += numOfDronesProvided;
            
            base.OnBuildableCompleted();
		}

		protected override void OnDestroyed()
		{
			_droneManager.NumOfDrones -= numOfDronesProvided;
            
            base.OnDestroyed();
		}
	}
}
