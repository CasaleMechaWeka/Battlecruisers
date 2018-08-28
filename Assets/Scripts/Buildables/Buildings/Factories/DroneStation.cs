using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
	{
		public int numOfDronesProvided;

        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Building4; } }
        public override TargetValue TargetValue { get { return TargetValue.Medium; } }

		protected override void OnBuildableCompleted()
		{
			_droneManager.NumOfDrones += numOfDronesProvided;
            
            base.OnBuildableCompleted();
		}

		protected override void OnDestroyed()
		{
            if (BuildableState == BuildableState.Completed)
            {
    			_droneManager.NumOfDrones -= numOfDronesProvided;
            }
            
            base.OnDestroyed();
		}
	}
}
