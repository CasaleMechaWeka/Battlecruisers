using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Factories;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    // TEMP  Turn this class off for final game :P
    public class Cheater : MonoBehaviour
    {
        private IFactoryProvider _factoryProvider;
        private float _lastGameSpeed;

        public int droneBoostNumber;
        public Canvas hudCanvas;

        public void Initialise(IFactoryProvider factoryProvider)
        {
            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
                return;
            }

            Helper.AssertIsNotNull(hudCanvas, factoryProvider);

            _factoryProvider = factoryProvider;
            _lastGameSpeed = 0;
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
            // N = Nuke
            else if (Input.GetKeyUp(KeyCode.N))
            {
                Vector2 nukePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _factoryProvider.PoolProviders.ExplosionPoolProvider.HugeExplosionsPool.GetItem(nukePoint);
            }
            // P = Pause
            else if (Input.GetKeyUp(KeyCode.P))
            {
                if (_lastGameSpeed == 0
                    && Time.timeScale != 0)
                {
                    _lastGameSpeed = Time.timeScale;
                    Time.timeScale = 0;
                }
                else if (_lastGameSpeed != 0
                    && Time.timeScale == 0)
                {
                    Time.timeScale = _lastGameSpeed;
                    _lastGameSpeed = 0;
                }
            }
        }

        private ICruiser FindCruiser(Faction faction)
        {
            ICruiser[] cruisers = FindObjectsOfType<Cruiser>();
            return cruisers.FirstOrDefault(cruiser => cruiser.Faction == faction);
        }
    }
}
