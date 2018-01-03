using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class TeslaCoil : TurretController
	{
        protected override Renderer GetBaseRenderer()
        {
            Renderer mainRenderer = GetComponent<Renderer>();
            Assert.IsNotNull(mainRenderer);
            return mainRenderer;
        }
	}
}
