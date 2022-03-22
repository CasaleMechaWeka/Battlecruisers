using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class GatlingMortar : OffenseTurret
	{
        // DLC  Have own sound
        protected override ISoundKey FiringSound => SoundKeys.Firing.AttackBoat;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Mortar;
    }
}
