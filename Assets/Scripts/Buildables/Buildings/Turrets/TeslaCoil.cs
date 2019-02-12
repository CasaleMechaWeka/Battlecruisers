using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class TeslaCoil : DefenseTurret
	{
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.TeslaCoil; } }

        protected override SpriteRenderer GetBaseRenderer()
        {
            SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(mainRenderer);
            return mainRenderer;
        }
	}
}
