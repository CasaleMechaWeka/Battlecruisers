using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class TargetProviderActivationArgs<TStats> : ProjectileActivationArgs<TStats> where TStats : IProjectileStats
    {
        ITarget Target { get; }
    }
}