using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetDetectorFactory
    {
        PvPManualDetectorProvider CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
        PvPManualDetectorProvider CreateFriendlyShipTargetDetector(ITransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
        PvPManualDetectorProvider CreateEnemyAircraftTargetDetector(ITransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
        PvPManualDetectorProvider CreateEnemyShipAndAircraftTargetDetector(ITransform parentTransform, float detectionRange, IPvPRangeCalculator rangeCalculator);
    }
}
