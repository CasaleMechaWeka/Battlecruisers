namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
    {
        public int numOfDronesProvided;

        public override TargetValue TargetValue => TargetValue.Medium;

        protected override void OnBuildableCompleted()
        {
            ParentCruiser.DroneManager.NumOfDrones += numOfDronesProvided;

            base.OnBuildableCompleted();
        }

        protected override void OnDestroyed()
        {
            if (BuildableState == BuildableState.Completed)
            {
                ParentCruiser.DroneManager.NumOfDrones -= numOfDronesProvided;
            }

            base.OnDestroyed();
        }
    }
}
