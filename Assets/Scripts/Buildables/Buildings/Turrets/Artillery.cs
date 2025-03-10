using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class Artillery : OffenseTurret
	{
        protected override ISoundKey FiringSound => SoundKeys.Firing.Artillery;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Artillery;
    }
}
