using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Debugging
{
    // TEMP  Turn this class off for final game :P
    public class Cheater : MonoBehaviour
    {
        private IFactoryProvider _factoryProvider;
        private ICruiser _playerCruiser, _aiCruiser;
        private float _lastGameSpeed;

        public int droneBoostNumber;
        public Canvas hudCanvas;

        public void Initialise(IFactoryProvider factoryProvider, ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Assert.IsNotNull(hudCanvas);
            Helper.AssertIsNotNull(hudCanvas, playerCruiser, aiCruiser);

            _factoryProvider = factoryProvider;
            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _lastGameSpeed = 0;

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
                if (_aiCruiser != null)
                {
                    _aiCruiser.TakeDamage(_aiCruiser.MaxHealth, null);
                }
            }
            // L = Loss
            else if (Input.GetKeyUp(KeyCode.L))
            {
                if (_playerCruiser != null)
                {
                    _playerCruiser.TakeDamage(_playerCruiser.MaxHealth, null);
                }
            }
            // B = Builders
            else if (Input.GetKeyUp(KeyCode.B))
            {
                if (_playerCruiser != null)
                {
                    _playerCruiser.DroneManager.NumOfDrones += droneBoostNumber;
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
    }
}
