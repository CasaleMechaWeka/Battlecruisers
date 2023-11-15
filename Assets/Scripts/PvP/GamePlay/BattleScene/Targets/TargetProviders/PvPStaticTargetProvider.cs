using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public class PvPStaticTargetProvider : IPvPTargetProvider
    {
        public IPvPTarget Target { get; }

        public PvPStaticTargetProvider(IPvPTarget target)
        {
            Target = target;
        }
    }
}
