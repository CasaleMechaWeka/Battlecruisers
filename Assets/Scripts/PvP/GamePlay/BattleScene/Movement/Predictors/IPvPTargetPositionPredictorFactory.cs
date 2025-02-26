using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public interface IPvPTargetPositionPredictorFactory
    {
        ITargetPositionPredictor CreateDummyPredictor();
        ITargetPositionPredictor CreateLinearPredictor();
        ITargetPositionPredictor CreateMortarPredictor();
    }

    public class PvPTargetPositionPredictorFactory : IPvPTargetPositionPredictorFactory
    {
        public ITargetPositionPredictor CreateDummyPredictor()
        {
            return new PvPDummyTargetPositionpredictor();
        }

        public ITargetPositionPredictor CreateLinearPredictor()
        {
            return new PvPLinearTargetPositionPredictor();
        }

        public ITargetPositionPredictor CreateMortarPredictor()
        {
            return new PvPMortarTargetPositionPredictor();
        }
    }
}
