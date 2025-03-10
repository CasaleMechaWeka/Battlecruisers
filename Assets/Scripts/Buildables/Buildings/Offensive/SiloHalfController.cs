using BattleCruisers.Movement.Rotation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class SiloHalfController : RotatingController
	{
		public SpriteRenderer Renderer { get; private set; }

		public void StaticInitialise()
		{
			Renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
			Assert.IsNotNull(Renderer);
		}
	}
}