using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
	{
		public int numOfDronesProvided;

        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Building4; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.DroneStation; } }
        public override TargetValue TargetValue { get { return TargetValue.Medium; } }

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.DroneStationBuildRateBoostProviders;
            }
        }

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
