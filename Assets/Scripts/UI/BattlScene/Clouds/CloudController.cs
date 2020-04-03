using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    // FELIX  Remove :D
    public class CloudController : MonoBehaviour, ICloud
    {
        private ICloudTeleporter _cloudTeleporter;

        public SpriteRenderer frontCloud;
        public SpriteRenderer backCloud;

        public Vector2 Size { get; private set; }

        public Vector2 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public void Initialise(ICloudStatsExtended cloudStats)
        {
            Assert.IsNotNull(cloudStats);
            Assert.IsNotNull(frontCloud);  // backCloud may be null for now

            // Find cloud size
            Size = frontCloud.size;
            frontCloud.color = cloudStats.FrontCloudColour;

            if (backCloud != null)
            {
                backCloud.color = cloudStats.BackCloudColour;
            }

            // Start moving
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(rigidBody);
            rigidBody.velocity = new Vector2(cloudStats.HorizontalMovementSpeedInMPerS, 0);

            if (cloudStats.HorizontalMovementSpeedInMPerS > 0)
            {
                // Left to right
                _cloudTeleporter = new PositiveVelocityTeleporter(this, cloudStats);
            }
            else
            {
                // Right to left
                _cloudTeleporter = new NegativeVelocityTeleporter(this, cloudStats);
            }
        }

        void Update()
        {
            if (_cloudTeleporter.ShouldTeleportCloud())
            {
                _cloudTeleporter.TeleportCloud();
            }
        }
    }
}
