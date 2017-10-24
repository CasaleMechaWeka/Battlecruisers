using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
	public class BomberStats : MonoBehaviour
	{
		public ProjectileController bombPrefab;
		public float damage;

		void Awake()
		{
			Assert.IsNotNull(bombPrefab);
			Assert.IsTrue(damage > 0);
		}
	}
}
