using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class AntiShipTurret : DefenseTurret
	{
        protected override ISoundKey FiringSound => SoundKeys.Firing.BigCannon;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.AntiShipTurret;
    }
}
