using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
	public class BomberStats : MonoBehaviour
	{
		public ShellController bombPrefab;
		public float damage;

		void Awake()
		{
			Assert.IsNotNull(bombPrefab);
			Assert.IsTrue(damage > 0);
		}
	}
}
