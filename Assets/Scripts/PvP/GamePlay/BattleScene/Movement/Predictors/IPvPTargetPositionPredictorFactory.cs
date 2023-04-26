namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public interface IPvPTargetPositionPredictorFactory
    {
        IPvPTargetPositionPredictor CreateDummyPredictor();
        IPvPTargetPositionPredictor CreateLinearPredictor();
        IPvPTargetPositionPredictor CreateMortarPredictor();
    }

    public class TargetPositionPredictorFactory : IPvPTargetPositionPredictorFactory
    {
        public IPvPTargetPositionPredictor CreateDummyPredictor()
        {
            return new PvPDummyTargetPositionpredictor();
        }

        public IPvPTargetPositionPredictor CreateLinearPredictor()
        {
            return new PvPLinearTargetPositionPredictor();
        }

        public IPvPTargetPositionPredictor CreateMortarPredictor()
        {
            return new PvPMortarTargetPositionPredictor();
        }
    }
}
