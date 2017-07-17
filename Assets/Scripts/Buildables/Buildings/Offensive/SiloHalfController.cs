using BattleCruisers.Buildables.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
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