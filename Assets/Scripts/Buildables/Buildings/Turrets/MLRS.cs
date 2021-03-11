using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class MLRS : OffenseTurret
    {
        // DLC  Have own sound
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.RocketLauncher;
        protected override ISoundKey FiringSound => SoundKeys.Firing.RocketLauncher;
    }
}