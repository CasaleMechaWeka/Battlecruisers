using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class AntiShipTurret : DefenseTurret
	{
        protected override ISoundKey FiringSound { get { return SoundKeys.Firing.BigCannon; } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.AntiShipTurret; } }
    }
}
