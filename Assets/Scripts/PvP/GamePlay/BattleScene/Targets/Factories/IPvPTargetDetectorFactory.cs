using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetDetectorFactory
    {
        PvPManualDetectorProvider CreateEnemyShipTargetDetector(IPvPTransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
        PvPManualDetectorProvider CreateFriendlyShipTargetDetector(IPvPTransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
        PvPManualDetectorProvider CreateEnemyAircraftTargetDetector(IPvPTransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
        PvPManualDetectorProvider CreateEnemyShipAndAircraftTargetDetector(IPvPTransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
    }
}
