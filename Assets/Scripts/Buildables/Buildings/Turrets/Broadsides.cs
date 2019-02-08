using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class Broadsides : OffenseTurret
	{
        protected override ISoundKey FiringSound { get { return SoundKeys.Firing.Broadsides; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Ultra; } }

        protected override IObservableCollection<IBoostProvider> BuildRateBoostProviders
        {
            get
            {
                return _factoryProvider.GlobalBoostProviders.UltrasBuildRateBoostProviders;
            }
        }
    }
}
