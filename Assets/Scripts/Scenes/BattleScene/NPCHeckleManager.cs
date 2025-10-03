using System;
using UnityEngine;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.UI.BattleScene.Heckles;
using BattleCruisers.Utils;

namespace BattleCruisers.Scenes.BattleScene
{
    /// <summary>
    /// Manages NPC heckles during single-player battles.
    /// Uses simple time-based triggers only (no event subscriptions).
    /// </summary>
    public class NPCHeckleManager : MonoBehaviour
    {
        private HeckleMessage _heckleMessage;
        private HeckleConfig _config;
        private Cruiser _aiCruiser;
        
        // State tracking
        private int _hecklesShown = 0;
        private int _currentHeckleIndex = 0;
        private float _nextHeckleTime = 0f;
        private float _battleStartTime = 0f;
        
        // Trigger flags
        private bool _hasTriggeredHealthThreshold = false;
        private bool _isInitialized = false;

        /// <summary>
        /// Initialize the heckle manager with minimal dependencies.
        /// </summary>
        /// <param name="heckleMessage">The UI component to display heckles</param>
        /// <param name="config">Configuration for heckles</param>
        /// <param name="aiCruiser">The enemy AI cruiser</param>
        /// <param name="playerCruiser">Not used anymore but kept for compatibility</param>
        public void Initialise(
            HeckleMessage heckleMessage, 
            HeckleConfig config, 
            Cruiser aiCruiser,
            Cruiser playerCruiser)
        {
            Helper.AssertIsNotNull(heckleMessage, config, aiCruiser);
            
            _heckleMessage = heckleMessage;
            _config = config;
            _aiCruiser = aiCruiser;
            
            if (!_config.enableHeckles)
            {
                enabled = false;
                return;
            }

            _battleStartTime = Time.time;
            _nextHeckleTime = _battleStartTime + UnityEngine.Random.Range(
                _config.minTimeBeforeFirstHeckle, 
                _config.maxTimeBeforeFirstHeckle);
            
            _isInitialized = true;
            Logging.Log(Tags.BATTLE_SCENE, $"NPCHeckleManager initialized. Max heckles: {_config.maxHeckles}");
        }

        private void Update()
        {
            if (!_isInitialized || !_config.enableHeckles)
                return;

            // Time-based heckle
            if (_hecklesShown < _config.maxHeckles && Time.time >= _nextHeckleTime)
            {
                ShowRandomHeckle();
                ScheduleNextTimeBasedHeckle();
            }

            // Health threshold check (using IDamagable.Health property)
            if (_config.enableHealthThresholdHeckle && 
                !_hasTriggeredHealthThreshold && 
                _hecklesShown < _config.maxHeckles)
            {
                float healthPercentage = _aiCruiser.Health / _aiCruiser.MaxHealth;
                if (healthPercentage <= _config.heckleOnHealthThreshold)
                {
                    ShowRandomHeckle();
                    _hasTriggeredHealthThreshold = true;
                }
            }
        }

        private void ShowRandomHeckle()
        {
            int heckleIndex = _config.GetNextHeckleIndex(ref _currentHeckleIndex);
            _heckleMessage.Show(heckleIndex);
            _hecklesShown++;
            
            Logging.Log(Tags.BATTLE_SCENE, 
                $"NPC Heckle shown ({_hecklesShown}/{_config.maxHeckles}): Index {heckleIndex}");
        }

        private void ScheduleNextTimeBasedHeckle()
        {
            _nextHeckleTime = Time.time + _config.minTimeBetweenHeckles;
        }
    }
}

