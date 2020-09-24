using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudController : MonoBehaviour, ICloud
    {
        public Vector2 Size { get; private set; }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public void Initialise(ICloudStats cloudStats)
        {
            Assert.IsNotNull(cloudStats);
            Assert.IsTrue(cloudStats.HorizontalMovementSpeedInMPerS > 0, "Only support clouds moving from left to right.");

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);

            renderer.color = cloudStats.CloudColour;
            renderer.transform.position = new Vector3(renderer.transform.position.x, cloudStats.CloudYPosition, cloudStats.CloudZPosition);

            float xRotationInDegrees = cloudStats.FlipClouds ? 180 : 0;
            renderer.transform.rotation = Quaternion.Euler(xRotationInDegrees, 0, 0);

            Size = renderer.bounds.size;

            // Start moving
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(rigidBody);
            rigidBody.velocity = new Vector2(cloudStats.HorizontalMovementSpeedInMPerS, 0);
        }
    }
}