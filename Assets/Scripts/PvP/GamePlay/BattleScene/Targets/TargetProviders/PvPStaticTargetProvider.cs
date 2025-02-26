using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public class PvPStaticTargetProvider : ITargetProvider
    {
        public ITarget Target { get; }

        public PvPStaticTargetProvider(ITarget target)
        {
            Target = target;
        }
    }
}
