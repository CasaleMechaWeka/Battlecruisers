using BattleCruisers.Effects;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class DronesTestGod : MonoBehaviour
    {
        public GameObject parentObject;
        public DroneController dronePrefab;
        public int numOfDrones = 10;
        public float spawnRadiusInM = 0.5f;

        void Start()
        {
            for (int i = 0; i < numOfDrones; ++i)
            {
                DroneController newDrone = Instantiate(dronePrefab);
                //DroneController newDrone = Instantiate(dronePrefab, parentObject.transform);

                newDrone.Initialise(RandomGenerator.Instance);
                newDrone.Activate(RandomisePosition(parentObject.transform.position));
                //newDrone.transform.position = RandomisePosition(parentObject.transform.position);

                Debug.Log($"Created rone #{i} at position: {newDrone.transform.position}");
            }
        }

        private Vector2 RandomisePosition(Vector2 originalPosition)
        {
            float x = RandomGenerator.Instance.Range(-spawnRadiusInM, spawnRadiusInM);
            float y = RandomGenerator.Instance.Range(-spawnRadiusInM, spawnRadiusInM);
            return new Vector2(originalPosition.x + x, originalPosition.y + y);
        }
    }
}