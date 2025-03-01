using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds
{
    public class PvPCloudRandomiser : ICloudRandomiser
    {
        private readonly IRandomGenerator _random;
        private readonly IRange<float> _rightCloudValidXPositions;

        public PvPCloudRandomiser(IRandomGenerator random, IRange<float> rightCloudValidXPositions)
        {
            PvPHelper.AssertIsNotNull(random, rightCloudValidXPositions);

            _random = random;
            _rightCloudValidXPositions = rightCloudValidXPositions;
        }

        public void RandomiseStartingPosition(ICloud leftCloud, ICloud rightCloud)
        {
            PvPHelper.AssertIsNotNull(leftCloud, rightCloud);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            float distanceBetweenCloudsInM = rightCloud.Position.x - leftCloud.Position.x;

            float rightCloudXPosition = _random.Range(_rightCloudValidXPositions);
            rightCloud.Position = new Vector3(rightCloudXPosition, rightCloud.Position.y, rightCloud.Position.z);

            float leftCloudXPosition = rightCloudXPosition - distanceBetweenCloudsInM;
            leftCloud.Position = new Vector3(leftCloudXPosition, leftCloud.Position.y, leftCloud.Position.z);

            // Logging.Log(Tags.CLOUDS, $"leftCloudPosition: {leftCloud.Position}  rightCloudPosition: {rightCloud.Position}");
        }
    }
}