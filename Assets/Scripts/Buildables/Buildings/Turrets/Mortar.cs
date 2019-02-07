using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class Mortar : DefenseTurret
	{
        protected override ISoundKey FiringSound { get { return SoundKeys.Firing.Artillery; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.Mortar; } }
    }
}
