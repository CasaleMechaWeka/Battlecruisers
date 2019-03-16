using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProviders
{
    public class StaticTargetProvider : ITargetProvider
    {
        public ITarget Target { get; }

        public StaticTargetProvider(ITarget target)
        {
            Target = target;
        }
    }
}
