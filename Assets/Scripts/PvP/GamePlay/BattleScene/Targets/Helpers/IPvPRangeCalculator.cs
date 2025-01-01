using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers
{
    public interface IPvPRangeCalculator
    {
        bool IsInRange(ITransform parentTransform, IPvPTarget target, float rangeInM);
    }
}