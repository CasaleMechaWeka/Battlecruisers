using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class TeslaCoil : DefenseTurret
	{
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Buildings.TeslaCoil; } }

        protected override Renderer GetBaseRenderer()
        {
            Renderer mainRenderer = GetComponent<Renderer>();
            Assert.IsNotNull(mainRenderer);
            return mainRenderer;
        }
	}
}
