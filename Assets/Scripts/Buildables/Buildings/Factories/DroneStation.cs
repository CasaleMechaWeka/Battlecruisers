using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
	{
		public int numOfDronesProvided;

        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Building4; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.DroneStation; } }
        public override TargetValue TargetValue { get { return TargetValue.Medium; } }

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders,
            IList<IObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_factoryProvider.GlobalBoostProviders.BuildRate.DroneBuildingsProviders);
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
