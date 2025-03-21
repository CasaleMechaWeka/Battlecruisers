using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class NukeController :
        ProjectileWithTrail<TargetProviderActivationArgs<INukeStats>, INukeStats>, 
        ITargetProvider
	{
		private INukeStats _nukeStats;
		private IFlightPointsProvider _flightPointsProvider;
        public SpriteRenderer nukeSprite; // Reference to the nuke's sprite

		public ITarget Target { get; private set; }

        public override void Activate(TargetProviderActivationArgs<INukeStats> activationArgs)
        {
            base.Activate(activationArgs);

			_nukeStats = activationArgs.ProjectileStats;
            _flightPointsProvider = _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;

			Target = activationArgs.Target;
            
            // Make sure the sprite is visible when activated
            if (nukeSprite != null)
            {
                nukeSprite.enabled = true;
            }
		}

		public void Launch()
		{
            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			MovementController 
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    _nukeStats.CruisingAltitudeInM, 
                    _flightPointsProvider);
		}
        
        // Add cleanup method to hide visual elements on detonation
        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            
            // Hide the sprite when the nuke detonates
            if (nukeSprite != null)
            {
                nukeSprite.enabled = false;
            }
            else
            {
                // If the specific sprite reference isn't set, try to find and disable all sprites
                SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.enabled = false;
                }
            }
        }
        
        // Backup method in case OnImpactCleanUp isn't called
        protected override void DestroyProjectile()
        {
            // Hide the sprite when the nuke is destroyed
            if (nukeSprite != null)
            {
                nukeSprite.enabled = false;
            }
            else
            {
                // If the specific sprite reference isn't set, try to find and disable all sprites
                SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.enabled = false;
                }
            }
            
            base.DestroyProjectile();
        }
	}
}
