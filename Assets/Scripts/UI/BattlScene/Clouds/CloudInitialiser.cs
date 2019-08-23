using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudInitialiser : MonoBehaviour
    {
        public void Initialise(ICloudGenerationStats cloudStats, IRandomGenerator random)
        {
            Helper.AssertIsNotNull(cloudStats, random);

            CloudFactory cloudFactory = GetComponent<CloudFactory>();
            Assert.IsNotNull(cloudFactory);
            cloudFactory.Initialise();

            ICloudGenerator cloudGenerator = new CloudGenerator(cloudFactory, random);
            cloudGenerator.GenerateClouds(cloudStats);
        }
    }
}