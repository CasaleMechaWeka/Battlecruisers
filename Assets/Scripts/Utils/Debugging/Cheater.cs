using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Debugging
{
    // TEMP  Turn this class off for final game :P
    public class Cheater : MonoBehaviour
    {
        public int droneBoostNumber;
        public Canvas hudCanvas;

        void Start()
        {
            Assert.IsNotNull(hudCanvas);

            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            // W = Win
            if (Input.GetKeyUp(KeyCode.W))
            {
                ICruiser aiCruiser = FindCruiser(Faction.Reds);

                if (aiCruiser != null)
                {
                    aiCruiser.TakeDamage(aiCruiser.MaxHealth, null);
                }
            }
            // L = Loss
            else if (Input.GetKeyUp(KeyCode.L))
            {
                ICruiser playerCruiser = FindCruiser(Faction.Blues);

                if (playerCruiser != null)
                {
                    playerCruiser.TakeDamage(playerCruiser.MaxHealth, null);
                }
            }
            // B = Builders
            else if (Input.GetKeyUp(KeyCode.B))
            {
                ICruiser playerCruiser = FindCruiser(Faction.Blues);

                if (playerCruiser != null)
                {
                    playerCruiser.DroneManager.NumOfDrones += droneBoostNumber;
                }
            }
            // T = Toggle UI
            else if (Input.GetKeyUp(KeyCode.T))
            {
                hudCanvas.gameObject.SetActive(!hudCanvas.gameObject.activeSelf);
            }
        }

        private ICruiser FindCruiser(Faction faction)
        {
            ICruiser[] cruisers = FindObjectsOfType<Cruiser>();
            return cruisers.FirstOrDefault(cruiser => cruiser.Faction == faction);
        }
    }
}
