using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class RocketTarget : Target
	{
		private Rigidbody2D _rigidBody;

		public override TargetType TargetType { get { return TargetType.Rocket; } }
		public override Vector2 Velocity { get { return _rigidBody.velocity; } }

        private Vector2 _size;
        public override Vector2 Size { get { return _size; } }

        public void Initialise(Faction faction, Rigidbody2D rigidBody, Vector2 size)
		{
			StaticInitialise();

			Faction = faction;
			_rigidBody = rigidBody;
            _size = size;
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