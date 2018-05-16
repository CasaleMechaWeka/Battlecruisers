using UnityEngine;

namespace BattleCruisers.Utils
{
	public interface IPositionClamper
	{
		void Clamp(Vector2 position);
		void Clamp(Vector3 position);
	}
}
