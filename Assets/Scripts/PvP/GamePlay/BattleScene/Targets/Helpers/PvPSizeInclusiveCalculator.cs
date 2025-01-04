using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers
{
    public class PvPSizeInclusiveCalculator : IRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM)
        {
            if (target == null || parentTransform == null)
            {//if either have been distoryed then it's not in range
                return false;
            }
            float distanceCenterToCenter = Vector2.Distance(target.Position, parentTransform.Position);
            float distanceCenterToEdge = distanceCenterToCenter - target.Size.x / 2;
            return distanceCenterToEdge <= rangeInM;
        }
    }
}