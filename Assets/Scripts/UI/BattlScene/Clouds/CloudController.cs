using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudController : MonoBehaviour, ICloud
    {
        private ICloudTeleporter _cloudTeleporter;

        public Vector2 Size { get; private set; }

        public Vector2 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public void Initialise(ICloudStats cloudStats)
        {
            Assert.IsNotNull(cloudStats);

            // Find cloud size
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);
            Size = renderer.size;

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
