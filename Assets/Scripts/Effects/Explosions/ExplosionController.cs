namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupController
    {
        public virtual IExplosion Initialise()
        {
            IBroadcastingParticleSystem[] particleSystems = GetParticleSystems();
            return new Explosion(this, particleSystems);
        }
    }
}