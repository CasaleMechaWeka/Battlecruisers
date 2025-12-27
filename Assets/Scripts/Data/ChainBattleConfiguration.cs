using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Sound;
using BattleCruisers.Scenes.BattleScene;

namespace BattleCruisers.Data
{
    [CreateAssetMenu(fileName = "ChainBattle", menuName = "BattleCruisers/ChainBattle Configuration")]
    public class ChainBattleConfiguration : ScriptableObject
    {
        [Header("Basic Info")]
        public int levelNumber;
        public string levelNameKey; // Localization key for the ChainBattle name (should match captain name)
        public bool playerTalksFirst; // Determines who speaks first in trash talk screen
        
        [Header("Battle Settings")]
        public SoundKeyPair musicKeys; // Music tracks for this ChainBattle
        public string skyMaterialName; // Sky material name
        [Range(0, 50)] public int captainExoId = 1; // Captain exoskeleton ID (same throughout all phases)

        [Header("Battle Structure")]
        public List<CruiserPhase> cruiserPhases = new List<CruiserPhase>();

        [Header("Legacy Reactive Behaviors (Use Phase-Specific Instead)")]
        [Tooltip("Legacy global reactive behaviors. Define reactive behaviors per-phase in CruiserPhase.phaseConditionalActions instead.")]
        public List<ConditionalAction> conditionalActions = new List<ConditionalAction>();


        [Header("Dialog & Story")]
        public List<ChainBattleChat> customChats = new List<ChainBattleChat>();
        public List<string> dialogKeys = new List<string>(); // Localization keys instead of raw text (legacy)
        public List<DialogTiming> dialogTimings = new List<DialogTiming>();

        // Validation
        public bool IsValid()
        {
            return levelNumber > 0 &&
                   cruiserPhases.Count >= 1 &&
                   !string.IsNullOrEmpty(levelNameKey) &&
                   cruiserPhases.Any(phase => phase.isFinalPhase); // Must have at least one final phase
        }
    }

    [System.Serializable]
    public class ConditionalAction
    {
        [Header("Trigger")]
        public BuildingKey playerBuildingTrigger; // Player building prefab to watch for
        
        [Header("Response")]
        public float delayAfterTrigger; // Wait time in seconds before executing response
        
        [Header("Slot Actions")]
        public List<SlotReplacementAction> slotActions = new List<SlotReplacementAction>(); // Actions to perform on specific slots
        
        [Header("Dialog")]
        public string chatKey; // Chat key in format "level{number}/{chatName}" to display after actions complete
    }
    
    [System.Serializable]
    public class SlotReplacementAction
    {
        public byte slotID; // Slot index to modify
        public BuildingKey replacementPrefab; // Building prefab to place in slot (null = just destroy)
        public bool ignoreDroneReq = false;
        public bool ignoreBuildTime = false;
    }

    [System.Serializable]
    public class DialogTiming
    {
        public float triggerTime;
        public string dialogKey;
        public DialogTrigger triggerType;
    }

    public enum DialogTrigger
    {
        TimeBased,
        PhaseTransition,
        BuildingDestroyed
    }
}
