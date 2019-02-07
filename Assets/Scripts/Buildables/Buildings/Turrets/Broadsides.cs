using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class Broadsides : OffenseTurret
	{
        protected override ISoundKey FiringSound { get { return SoundKeys.Firing.Broadsides; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Ultra; } }
    }
}
