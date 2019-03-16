using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class RocketTarget : Target
	{
		private Rigidbody2D _rigidBody;

		public override TargetType TargetType => TargetType.Rocket;
		public override Vector2 Velocity => _rigidBody.velocity;

        private Vector2 _size;
        public override Vector2 Size => _size;

        public void Initialise(Faction faction, Rigidbody2D rigidBody)
		{
			StaticInitialise();

			Faction = faction;
			_rigidBody = rigidBody;

            SpriteRenderer rocketRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(rocketRenderer);

            _size = rocketRenderer.bounds.size;
		}

		// All RocketTarget gameObjects are wrapped by a RocketController gameObject.
		// Hence, we need to destroy the parent gameObject.
		protected override void InternalDestroy()
		{
			Assert.IsNotNull(transform.parent);
			Destroy(transform.parent.gameObject);
		}
	}
}