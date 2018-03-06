using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudGenerator : ICloudGenerator
    {
        private readonly ICloudFactory _cloudFactory;

        public CloudGenerator(ICloudFactory cloudFactory)
        {
            Assert.IsNotNull(cloudFactory);
            _cloudFactory = cloudFactory;
        }

        public void GenerateClouds(
            Rect cloudArea, 
            CloudDensity density = CloudDensity.Medium, 
            CloudMovementSpeed movementSpeed = CloudMovementSpeed.Slow)
        {
            throw new System.NotImplementedException();
        }
    }
}
