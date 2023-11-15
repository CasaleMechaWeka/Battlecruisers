using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;

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
