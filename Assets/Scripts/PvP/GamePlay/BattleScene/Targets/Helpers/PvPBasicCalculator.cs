using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers
{
    public class PvPBasicCalculator : IPvPRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, IPvPTarget target, float rangeInM)
        {
            return Vector2.Distance(target.Transform.Position, parentTransform.Position) <= rangeInM;
        }
    }
}