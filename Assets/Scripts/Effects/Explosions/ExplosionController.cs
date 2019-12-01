namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupInitialiser
    {
        public virtual IExplosion Initialise()
        {
            IBroadcastingParticleSystem[] particleSystems = GetParticleSystems();
            return new Explosion(this, particleSystems);
        }
    }
}