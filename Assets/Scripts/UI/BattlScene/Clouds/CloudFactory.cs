using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudFactory : MonoBehaviour, ICloudFactory
    {
        public List<CloudController> cloudPrefabs;

        public void Initialise()
        {
            Assert.IsTrue(cloudPrefabs.Count != 0);
        }

        public ICloud CreateCloud(Vector3 spawnPosition)
        {
            CloudController cloudToCreate = cloudPrefabs.Shuffle().First();
            return Instantiate(cloudToCreate, spawnPosition, new Quaternion());
        }

        public ICloudStats CreateCloudStats(ICloudGenerationStats generationStats)
        {
            return new CloudStats(generationStats);
        }
    }
}
