using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudRandomiser : ICloudRandomiser
    {
        private readonly IRandomGenerator _random;
        private readonly IRange<float> _rightCloudValidXPositions;

        public CloudRandomiser(IRandomGenerator random, IRange<float> rightCloudValidXPositions)
        {
            Helper.AssertIsNotNull(random, rightCloudValidXPositions);

            _random = random;
            _rightCloudValidXPositions = rightCloudValidXPositions;
        }

        public void RandomiseStartingPosition(ICloudNEW leftCloud, ICloudNEW rightCloud)
        {
            Helper.AssertIsNotNull(leftCloud, rightCloud);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            float distanceBetweenCloudsInM = rightCloud.Position.x - leftCloud.Position.x;

            float rightCloudXPosition = _random.Range(_rightCloudValidXPositions);
            rightCloud.Position = new Vector2(rightCloudXPosition, rightCloud.Position.y);

            float leftCloudXPosition = rightCloudXPosition - distanceBetweenCloudsInM;
            leftCloud.Position = new Vector2(leftCloudXPosition, leftCloud.Position.y);

            Logging.Log(Tags.CLOUDS, $"leftCloudPosition: {leftCloud.Position}  rightCloudPosition: {rightCloud.Position}");
        }
    }
}