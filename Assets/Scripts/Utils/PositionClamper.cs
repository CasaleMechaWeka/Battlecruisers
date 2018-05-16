using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
{
	public class PositionClamper : IPositionClamper
	{
		private readonly Rectangle _bounds;

		public PositionClamper(Rectangle bounds)
		{
			Assert.IsNotNull(bounds);
			_bounds = bounds;
		}

		public Vector2 Clamp(Vector2 position)
		{
			position.x = Mathf.Clamp(position.x, _bounds.MinX, _bounds.MaxX);
            position.y = Mathf.Clamp(position.y, _bounds.MinY, _bounds.MaxY);

			return position;
		}

		public Vector3 Clamp(Vector3 position)
		{
			position.x = Mathf.Clamp(position.x, _bounds.MinX, _bounds.MaxX);
            position.y = Mathf.Clamp(position.y, _bounds.MinY, _bounds.MaxY);

			return position;
		}
	}
}
