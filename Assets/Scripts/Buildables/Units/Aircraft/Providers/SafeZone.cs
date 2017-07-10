using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.Providers
{
	public class SafeZone
	{
		public float MinX { get; private set; }
		public float MaxX { get; private set; }
		public float MinY { get; private set; }
		public float MaxY { get; private set; }

		public SafeZone(float minX, float maxX, float minY, float maxY)
		{
			Assert.IsTrue(minX < maxX);
			Assert.IsTrue(minY < maxY);

			MinX = minX;
			MaxX = maxX;
			MinY = minY;
			MaxY = maxY;
		}
	}
}
