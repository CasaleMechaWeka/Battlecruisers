using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudController : MonoBehaviour, ICloud
    {
        public SpriteRenderer frontCloud;
        public SpriteRenderer backCloud;

        public Vector2 Size { get; private set; }

        public Vector2 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public void Initialise(ICloudStats cloudStats)
        {
            Assert.IsNotNull(cloudStats);
            Assert.IsTrue(cloudStats.HorizontalMovementSpeedInMPerS > 0, "Only support clouds moving from left to right.");
            Helper.AssertIsNotNull(frontCloud, backCloud);

            frontCloud.color = cloudStats.CloudColour;
            frontCloud.transform.position = new Vector3(frontCloud.transform.position.x, cloudStats.Height, frontCloud.transform.position.z);

            backCloud.color = cloudStats.MistColour;
            backCloud.transform.position = new Vector3(backCloud.transform.position.x, cloudStats.Height, backCloud.transform.position.z);

            Size = frontCloud.bounds.size;

            // Start moving
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(rigidBody);
            rigidBody.velocity = new Vector2(cloudStats.HorizontalMovementSpeedInMPerS, 0);
        }
    }
}