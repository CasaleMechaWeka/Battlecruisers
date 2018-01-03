using BattleCruisers.Buildables.Buildings.Turrets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    // FELIX  Move to turrets folder :/
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
