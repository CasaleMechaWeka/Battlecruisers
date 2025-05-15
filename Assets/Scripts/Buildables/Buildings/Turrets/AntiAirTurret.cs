using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class AntiAirTurret : DefenseTurret
    {
        protected override SoundKey FiringSound => SoundKeys.Firing.AntiAir;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.AntiAirTurret;
    }
}
