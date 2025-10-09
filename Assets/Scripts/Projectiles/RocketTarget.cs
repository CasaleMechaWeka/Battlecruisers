using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
	public class RocketTarget : Target
	{
		private Rigidbody2D _rigidBody;
		private IRemovable _parentProjectile;

		public override TargetType TargetType => TargetType.Rocket;
		public override Vector2 Velocity => _rigidBody.velocity;

		private Vector2 _size;
		public override Vector2 Size => _size;

		public void Initialise(Faction faction, Rigidbody2D rigidBody, IRemovable parentProjectile)
		{
			Helper.AssertIsNotNull(rigidBody, parentProjectile);

			StaticInitialise();

			Faction = faction;
			_rigidBody = rigidBody;
			_parentProjectile = parentProjectile;

			BoxCollider2D collider = GetComponent<BoxCollider2D>();
			Assert.IsNotNull(collider);

			_size = collider.size;
		}

		protected override void InternalDestroy()
		{
			_parentProjectile.RemoveFromScene();
		}
	}
}