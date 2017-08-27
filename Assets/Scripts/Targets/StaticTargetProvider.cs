using BattleCruisers.Buildables;

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
