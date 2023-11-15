using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public class PvPFlightPointStats
    {
        public float AscendPointRadiusVariationM { get; }
        public float DescendPointRadiusVariationM { get; }
        public float TargetPointXRadiusVariationM { get; }

        public PvPFlightPointStats(float ascendPointRadiusVariationM, float descendPointRadiusVariationM, float targetPointXRadiusVariationM)
        {
            Assert.IsTrue(ascendPointRadiusVariationM > 0);
            Assert.IsTrue(descendPointRadiusVariationM > 0);
            Assert.IsTrue(targetPointXRadiusVariationM > 0);

            AscendPointRadiusVariationM = ascendPointRadiusVariationM;
            DescendPointRadiusVariationM = descendPointRadiusVariationM;
            TargetPointXRadiusVariationM = targetPointXRadiusVariationM;
        }
    }
}