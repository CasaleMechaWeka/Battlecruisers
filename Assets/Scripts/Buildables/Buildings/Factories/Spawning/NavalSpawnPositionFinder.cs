using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Factories.Spawning
{
    public class NavalSpawnPositionFinder : ISpawnPositionFinder
    {
        private readonly IFactory _factory;

        public Vector3 FindSpawnPosition(IUnit unitToSpawn)
        {
            //float horizontalChange = (_factory.Size.x * 0.6f) + (unitToSpawn.Size.x * 0.5f);

            //// If the factory is facing left it has been mirrored (rotated
            //// around the y-axis by 180*).  So its right is an unmirrored
            //// factory's left :/
            //Vector3 direction = _factory.Transform transform.right;

            //return transform.position + (direction * horizontalChange);

            // FELIX :P
            return default(Vector3);
        }
    }
}