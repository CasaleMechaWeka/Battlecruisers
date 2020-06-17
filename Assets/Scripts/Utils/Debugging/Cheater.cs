using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Debugging
{
    public class Cheater : CheaterBase, ICheater
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
        }

        public void Win()
        {
            if (_aiCruiser != null)
            {
                _aiCruiser.TakeDamage(_aiCruiser.MaxHealth, null);
            }
        }

        public void Lose()
        {
            if (_playerCruiser != null)
            {
                _playerCruiser.TakeDamage(_playerCruiser.MaxHealth, null);
            }
        }

        public void AddBuilders()
        {
            if (_playerCruiser != null)
            {
                _playerCruiser.DroneManager.NumOfDrones += droneBoostNumber;
            }
        }

        public void ToggleUI()
        {
            hudCanvas.gameObject.SetActive(!hudCanvas.gameObject.activeSelf);
        }

        public void ShowNuke()
        {
            Vector2 nukePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _factoryProvider.PoolProviders.ExplosionPoolProvider.HugeExplosionsPool.GetItem(nukePoint);
        }

        public void TogglePause()
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

        public void ChangeBuildSpeed(BuildSpeed buildSpeed)
        {
            BuildProgressCalculatorFactory.playerBuildSpeed.BuildSpeed = buildSpeed;
            BuildProgressCalculatorFactory.aiBuildSpeed.BuildSpeed = buildSpeed;
        }
    }
}