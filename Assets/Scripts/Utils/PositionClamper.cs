using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
{
	// FELIX  Test :D
	public class PositionClamper : IPositionClamper
	{
		private readonly Rectangle _bounds;

		public PositionClamper(Rectangle bounds)
		{
			Assert.IsNotNull(bounds);
			_bounds = bounds;
		}

		public void Clamp(Vector2 position)
		{
			position.x = Mathf.Clamp(position.x,_bounds.MinX,_bounds.MaxX);
            position.y = Mathf.Clamp(position.y,_bounds.MinY,_bounds.MaxY);
		}

		public void Clamp(Vector3 position)
		{
			position.x = Mathf.Clamp(position.x, _bounds.MinX, _bounds.MaxX);
            position.y = Mathf.Clamp(position.y, _bounds.MinY, _bounds.MaxY);
		}
	}
}
