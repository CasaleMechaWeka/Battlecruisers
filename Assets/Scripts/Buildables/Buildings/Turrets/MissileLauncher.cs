using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class MissileLauncher : DefenseTurret
	{
        // DLC  Have own sound
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.TeslaCoil;

        // FELIX  Avoid duplicate code with TeslaCoil
        protected override SpriteRenderer[] GetBaseRenderers()
        {
            SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(mainRenderer);
            return new SpriteRenderer[]
            {
                mainRenderer
            };
        }
	}
}
