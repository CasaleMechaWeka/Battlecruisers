using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data
{
    /// <summary>
    /// Configuration for NPC heckles during battle.
    /// Can be attached to Level or SideQuestData.
    /// </summary>
    [Serializable]
    public class HeckleConfig
    {
        [Header("General Settings")]
        [Tooltip("Enable heckles for this level")]
        public bool enableHeckles = false;

        [Tooltip("Maximum number of heckles to show during the battle")]
        [Range(0, 10)]
        public int maxHeckles = 3;

        [Header("Time-Based Triggers")]
        [Tooltip("Minimum time (seconds) before first heckle")]
        [Range(5f, 60f)]
        public float minTimeBeforeFirstHeckle = 10f;

        [Tooltip("Maximum time (seconds) before first heckle")]
        [Range(10f, 120f)]
        public float maxTimeBeforeFirstHeckle = 50f;

        [Tooltip("Minimum time (seconds) between heckles")]
        [Range(5f, 60f)]
        public float minTimeBetweenHeckles = 15f;

        [Header("Event-Based Triggers")]
        [Tooltip("Show heckle when enemy takes damage for the first time")]
        public bool heckleOnFirstDamage = false;

        [Tooltip("Show heckle when enemy health drops below this percentage")]
        [Range(0f, 1f)]
        public float heckleOnHealthThreshold = 0.25f;

        [Tooltip("Enable health threshold heckle")]
        public bool enableHealthThresholdHeckle = false;

        [Tooltip("Show heckle when player cruiser takes heavy damage")]
        public bool heckleOnPlayerDamaged = false;

        [Tooltip("Show heckle when enemy destroys a building")]
        public bool heckleOnBuildingDestroyed = false;

        [Header("Specific Heckles (Optional)")]
        [Tooltip("Leave empty to use random heckles. Otherwise, these specific heckle indices will be used in order.")]
        public List<int> specificHeckleIndices = new List<int>();

        /// <summary>
        /// Get a random heckle index.
        /// If specific heckles are configured, returns the next one in sequence.
        /// Otherwise returns a random index from 0-279.
        /// </summary>
        public int GetNextHeckleIndex(ref int currentIndex)
        {
            if (specificHeckleIndices != null && specificHeckleIndices.Count > 0)
            {
                int index = specificHeckleIndices[currentIndex % specificHeckleIndices.Count];
                currentIndex++;
                return index;
            }
            
            return UnityEngine.Random.Range(0, 280);
        }
    }
}

