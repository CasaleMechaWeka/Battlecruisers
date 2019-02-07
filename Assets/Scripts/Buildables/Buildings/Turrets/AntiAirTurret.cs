using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class AntiAirTurret : DefenseTurret
	{
        protected override ISoundKey FiringSound { get { return SoundKeys.Firing.AntiAir; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.AntiAirTurret; } }
    }
}
