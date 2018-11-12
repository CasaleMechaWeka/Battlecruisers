using BattleCruisers.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudInitialiser : MonoBehaviour
    {
        public void Initialise(ILevel level)
        {
            CloudFactory cloudFactory = GetComponent<CloudFactory>();
            Assert.IsNotNull(cloudFactory);
            cloudFactory.Initialise();

            ICloudGenerator cloudGenerator = new CloudGenerator(cloudFactory);
            cloudGenerator.GenerateClouds(level.CloudStats);
        }
    }
}