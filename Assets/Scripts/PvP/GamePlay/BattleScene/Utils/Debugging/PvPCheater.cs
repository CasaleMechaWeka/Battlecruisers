using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Debugging
{
    public class PvPCheater : PvPCheaterBase, IPvPCheater
    {
        private IPvPFactoryProvider _factoryProvider;
        private IPvPCruiser _playerCruiser, _aiCruiser;
        private float _lastGameSpeed;

        public int droneBoostNumber;
        public Canvas hudCanvas;

        public void Initialise(IPvPFactoryProvider factoryProvider, IPvPCruiser playerCruiser, IPvPCruiser aiCruiser)
        {
            Assert.IsNotNull(hudCanvas);
            PvPHelper.AssertIsNotNull(hudCanvas, playerCruiser, aiCruiser);

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

        public void SetSpeedNormal()
        {
            SetSpeed(PvPBuildSpeed.InfinitelySlow);
            PvPTaskFactory.delayProvider.DelayInS = PvPTaskFactory.DEFAULT_DELAY_IN_S;
        }

        public void SetSpeedFast()
        {
            SetSpeed(PvPBuildSpeed.Normal);
            PvPTaskFactory.delayProvider.DelayInS = PvPTaskFactory.MIN_DELAY_IN_S;
        }

        public void SetSpeedVeryFast()
        {
            SetSpeed(PvPBuildSpeed.VeryFast);
            PvPTaskFactory.delayProvider.DelayInS = PvPTaskFactory.MIN_DELAY_IN_S;
        }

        private void SetSpeed(PvPBuildSpeed buildSpeed)
        {
            PvPBuildProgressCalculatorFactory.playerABuildSpeed.BuildSpeed = buildSpeed;
            PvPBuildProgressCalculatorFactory.playerBBuildSpeed.BuildSpeed = buildSpeed;
            PvPBuildProgressCalculatorFactory.aiBuildSpeed.BuildSpeed = buildSpeed;
        }

        public void ToggleCursor()
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}