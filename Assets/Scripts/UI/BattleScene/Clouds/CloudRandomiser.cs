using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudRandomiser
    {
        private readonly IRange<float> _rightCloudValidXPositions;

        public CloudRandomiser(IRange<float> rightCloudValidXPositions)
        {
            Helper.AssertIsNotNull(rightCloudValidXPositions);

            _rightCloudValidXPositions = rightCloudValidXPositions;
        }

        public void RandomiseStartingPosition(CloudController leftCloud, CloudController rightCloud)
        {
            Helper.AssertIsNotNull(leftCloud, rightCloud);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            float distanceBetweenCloudsInM = rightCloud.Position.x - leftCloud.Position.x;

            float rightCloudXPosition = RandomGenerator.Range(_rightCloudValidXPositions);
            rightCloud.Position = new Vector3(rightCloudXPosition, rightCloud.Position.y, rightCloud.Position.z);

            float leftCloudXPosition = rightCloudXPosition - distanceBetweenCloudsInM;
            leftCloud.Position = new Vector3(leftCloudXPosition, leftCloud.Position.y, leftCloud.Position.z);

            Logging.Log(Tags.CLOUDS, $"leftCloudPosition: {leftCloud.Position}  rightCloudPosition: {rightCloud.Position}");
        }
    }
}