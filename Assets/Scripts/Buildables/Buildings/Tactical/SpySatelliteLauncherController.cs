using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class SpySatelliteLauncherController : SatelliteLauncherController, ISpySatelliteLauncher
	{
		protected override Vector3 SpawnPositionAdjustment { get { return new Vector3(0, 0.121f, 0); } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.SpySatellite; } }
        public override TargetValue TargetValue { get { return TargetValue.Medium; } }

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.TacticalsBuildRateBoostProviders;
            }
        }
    }
}
