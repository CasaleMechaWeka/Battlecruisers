using UnityEngine;
using UnityEngine.Assertions;

// FELIX  Move to Stats namespace :/
namespace BattleCruisers.Projectiles
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
