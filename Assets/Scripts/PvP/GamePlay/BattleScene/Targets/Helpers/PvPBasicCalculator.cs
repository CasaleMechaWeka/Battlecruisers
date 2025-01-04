using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers
{
    public class PvPBasicCalculator : IRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM)
        {
            return Vector2.Distance(target.Transform.Position, parentTransform.Position) <= rangeInM;
        }
    }
}