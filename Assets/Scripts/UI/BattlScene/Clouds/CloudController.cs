using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudController : MonoBehaviour, ICloud
    {
        public Vector2 Size { get; private set; }

        public void Initialise(ICloudStats cloudStats)
        {
            Assert.IsNotNull(cloudStats);

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);
            Size = renderer.size;

            // Set movement speed.  Need rigidbody?
        }

        // FELIX  Don't have to execute on every update, every 30th?  60th?
        void Update()
        {
            // Handle disappearing and reappearing :)
        }
    }
}
