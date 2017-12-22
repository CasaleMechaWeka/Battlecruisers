using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Targets
{
    public class StaticTargetProvider : ITargetProvider
    {
        public ITarget Target { get; private set; }

        public StaticTargetProvider(ITarget target)
        {
            Target = target;
        }
    }
}
