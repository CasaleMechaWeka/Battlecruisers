using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class MissileLauncher : DefenseTurret
	{
        // DLC  Have own sound
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.TeslaCoil;
        protected override bool HasSingleSprite => true;
	}
}
