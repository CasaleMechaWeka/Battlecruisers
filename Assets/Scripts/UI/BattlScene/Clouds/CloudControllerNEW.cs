using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudControllerNEW : MonoBehaviour, ICloudNEW
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

            frontCloud.color = cloudStats.FrontCloudColour;
            backCloud.color = cloudStats.BackCloudColour;

            Size = frontCloud.size;

            // Start moving
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(rigidBody);
            rigidBody.velocity = new Vector2(cloudStats.HorizontalMovementSpeedInMPerS, 0);
        }
    }
}