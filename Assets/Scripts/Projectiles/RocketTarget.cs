using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Localisation;
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

        public void Initialise(ILocTable commonStrings, Faction faction, Rigidbody2D rigidBody, IRemovable parentProjectile)
		{
            Helper.AssertIsNotNull(rigidBody, parentProjectile);

			StaticInitialise(commonStrings);

			Faction = faction;
			_rigidBody = rigidBody;
            _parentProjectile = parentProjectile;

            SpriteRenderer rocketRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(rocketRenderer);

            _size = rocketRenderer.bounds.size;
		}

		protected override void InternalDestroy()
		{
            _parentProjectile.RemoveFromScene();
		}
	}
}