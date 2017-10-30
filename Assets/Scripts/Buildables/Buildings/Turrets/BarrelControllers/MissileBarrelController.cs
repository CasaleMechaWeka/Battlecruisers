using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class MissileBarrelController : BarrelController
	{
        private ICircularList<MissileSpawner> _missileSpawners;

        protected override Vector3 ProjectileSpawnerPosition { get { return _missileSpawners.Items.Middle().transform.position; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            MissileSpawner[] missileSpawners = gameObject.GetComponentsInChildren<MissileSpawner>();
            Assert.IsTrue(missileSpawners.Length != 0);
            _missileSpawners = new CircularList<MissileSpawner>(missileSpawners);
        }

        public override void Initialise(IBarrelControllerArgs args)
		{
            base.Initialise(args);

            foreach (MissileSpawner missileSpawner in _missileSpawners.Items)
            {
                missileSpawner.Initialise(_projectileStats, args.FactoryProvider);
			}
		}

		protected override void Fire(float angleInDegrees)
		{
            Logging.Log(Tags.BARREL_CONTROLLER, "MissileBarrelController.Fire()  angleInDegrees: " + angleInDegrees);

            _missileSpawners.Next().SpawnMissile(
                angleInDegrees,
                IsSourceMirrored,
                Target,
                _targetFilter);
		}
	}
}
