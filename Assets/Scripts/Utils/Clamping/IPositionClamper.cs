using UnityEngine;

namespace BattleCruisers.Utils.Clamping
{
	public interface IPositionClamper
	{
		Vector2 Clamp(Vector2 position);
		Vector3 Clamp(Vector3 position);
	}
}
