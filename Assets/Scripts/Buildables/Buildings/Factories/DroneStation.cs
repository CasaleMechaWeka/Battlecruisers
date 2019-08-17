using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
	{
		public int numOfDronesProvided;

        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Building4;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.DroneStation;
        public override TargetValue TargetValue => TargetValue.Medium;

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.DroneBuildingsProviders);
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
