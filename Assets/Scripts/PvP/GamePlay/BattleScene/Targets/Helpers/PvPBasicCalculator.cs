using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers
{
    public class PvPBasicCalculator : IPvPRangeCalculator
    {
        public bool IsInRange(IPvPTransform parentTransform, IPvPTarget target, float rangeInM)
        {
            return Vector2.Distance(target.Transform.Position, parentTransform.Position) <= rangeInM;
        }
    }
}