using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Data;
using BattleCruisers.Cruisers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils;

namespace BattleCruisers.Scenes.BattleScene
{
    public class ChainBattleManager : MonoBehaviour
    {
        private ChainBattleConfiguration config;
        private Cruiser originalEnemyCruiser; // Reference to the original enemy cruiser from the scene

        public void SetConfiguration(ChainBattleConfiguration config)
        {
            this.config = config;

            // For demo purposes, inject localization strings for ChainBattle dialogs
            if (config.levelNumber == 32) // Demo level
            {
                InjectDemoLocalizationStrings();
            }
        }

        // Called by BattleSceneGod to provide references to existing cruisers
        public void InitializeCruisers(Cruiser playerCruiser, Cruiser enemyCruiser)
        {
            this.originalEnemyCruiser = enemyCruiser;
            this.enemyCruiser = enemyCruiser; // Start with original cruiser
        }

        void InjectDemoLocalizationStrings()
        {
            // Inject demo localization strings into the runtime localization table
            var demoStrings = new Dictionary<string, string>
            {
                { "CHAINBATTLE_DEMO_NAME", "Demo ChainBattle" },
                { "CHAINBATTLE_PHASE1_INTRO", "You think you can challenge me? Behold my first form!" },
                { "CHAINBATTLE_PHASE1_TAUNT", "My Trident hull will crush your pathetic fleet!" },
                { "CHAINBATTLE_PHASE2_INTRO", "Impressive... but not enough! Time to upgrade!" },
                { "CHAINBATTLE_PHASE3_INTRO", "You have pushed me to my limits! Now face my true power!" },
                { "CHAINBATTLE_PHASE3_FINALWARNING", "This is the end! My Man-of-War will be your doom!" },
                { "CHAINBATTLE_REACTION_AIRFACTORY", "An Air Factory? How quaint. Let me show you REAL air power!" },
                { "CHAINBATTLE_REACTION_NAVALFACTORY", "Building naval units? I'll counter with coastal defenses!" }
            };

            foreach (var kvp in demoStrings)
            {
                // Note: This would need the LocTableCache to support runtime injection
                // For now, we'll handle this in the dialog display
                Debug.Log($"Demo localization: {kvp.Key} = {kvp.Value}");
            }
        }
        private BattleSequencer battleSequencer;
        private Cruiser enemyCruiser;
        private int currentPhaseIndex = 0;
        private Dictionary<string, ChainBattleChat> chatLookup = new Dictionary<string, ChainBattleChat>();

        // Pre-instantiated phase cruisers (inactive)
        private List<GameObject> phaseCruisers = new List<GameObject>();

        // Reactive monitoring - simple building tracking
        private HashSet<IPrefabKey> playerBuiltBuildings = new HashSet<IPrefabKey>();

        void Start()
        {
            PreInstantiatePhases();
            StartCurrentPhase();
        }

        void PreInstantiatePhases()
        {
            // Skip phase 0 as it uses the original cruiser
            for (int i = 1; i < config.cruiserPhases.Count; i++)
            {
                var phase = config.cruiserPhases[i];

                // Create inactive GameObject for each phase (starting from phase 1)
                GameObject phaseObj = new GameObject($"Phase{i}_Cruiser");
                phaseObj.SetActive(false);

                // Instantiate hull prefab - keep Cruiser script disabled initially
                var hullPrefab = PrefabFactory.GetCruiserPrefab(phase.hullKey);
                var hullInstance = Instantiate(hullPrefab, phaseObj.transform);
                hullInstance.transform.localPosition = Vector3.zero;

                // Disable Cruiser script initially - will be enabled when phase becomes active
                var cruiserScript = hullInstance.GetComponent<Cruiser>();
                if (cruiserScript != null)
                {
                    cruiserScript.enabled = false;
                }

                phaseCruisers.Add(phaseObj);
            }
        }

        void StartCurrentPhase()
        {
            if (currentPhaseIndex >= config.cruiserPhases.Count) return;

            var phase = config.cruiserPhases[currentPhaseIndex];

            // For phase 0, use the original cruiser. For later phases, switch to phase cruiser
            if (currentPhaseIndex == 0)
            {
                // Phase 0 uses the original enemy cruiser
                enemyCruiser = originalEnemyCruiser;
                // Apply initial setup to original cruiser
                StartPhaseSequence(phase);
            }
            else
            {
                // Later phases use dedicated phase cruisers (index is currentPhaseIndex - 1 since phase 0 uses original)
                int phaseCruiserIndex = currentPhaseIndex - 1;
                if (phaseCruiserIndex < phaseCruisers.Count)
                {
                    var phaseObj = phaseCruisers[phaseCruiserIndex];
                    phaseObj.transform.position = CalculateEnemyPosition();
                    phaseObj.SetActive(true);

                    // Get the Cruiser component from the phase GameObject
                    var phaseCruiser = phaseObj.GetComponentInChildren<Cruiser>();
                    if (phaseCruiser != null)
                    {
                        enemyCruiser = phaseCruiser;
                        // Enable the cruiser script for the active phase
                        phaseCruiser.enabled = true;
                        // Apply initial setup to phase cruiser
                        StartPhaseSequence(phase);
                    }
                }
            }

            // Apply phase bonuses to the ACTIVE enemy cruiser
            foreach (var bonus in phase.phaseBonuses)
            {
                enemyCruiser.AddBoost(bonus);
            }

            // Start monitoring for reactive triggers
            StartReactiveMonitoring();
        }

        void StartPhaseSequence(CruiserPhase phase)
        {
            // Show phase start chats
            if (phase.phaseStartChats != null && phase.phaseStartChats.Count > 0)
            {
                var heckleManager = FindObjectOfType<ExtendedNPCHeckleManager>();
                if (heckleManager != null)
                {
                    foreach (var chat in phase.phaseStartChats)
                    {
                        heckleManager.ShowChainBattleChat(chat.chatKey, chat.speaker, chat.displayDuration);
                    }
                }
            }

            // Add phase-specific sequence points to BattleSequencer
            foreach (var seqPoint in phase.phaseSequencePoints)
            {
                // Convert to runtime SequencePoint and add to BattleSequencer
                // This requires extending BattleSequencer to accept runtime additions
            }

            // Spawn initial buildings and units
            foreach (var buildingAction in phase.initialBuildings)
            {
                ExecuteBuildingAction(buildingAction);
            }

            foreach (var unitAction in phase.initialUnits)
            {
                ExecuteUnitAction(unitAction);
            }
        }

        void StartReactiveMonitoring()
        {
            // Reactive monitoring happens in Update() by checking conditional actions
        }

        // Health monitoring and reactive actions
        void Update()
        {
            if (enemyCruiser != null)
            {
                // Phase transition check
                if (currentPhaseIndex < config.cruiserPhases.Count - 1)
                {
                    var phase = config.cruiserPhases[currentPhaseIndex];
                    if (!phase.isFinalPhase && enemyCruiser.Health <= 1)
                    {
                        StartTransition();
                    }
                }

                // Reactive actions check (phase-specific)
                CheckConditionalActions();
            }
        }

        void CheckConditionalActions()
        {
            // Check phase-specific conditional actions first
            var currentPhase = config.cruiserPhases[currentPhaseIndex];
            bool hasPhaseSpecificActions = false;

            if (currentPhase.phaseConditionalActions != null && currentPhase.phaseConditionalActions.Length > 0)
            {
                hasPhaseSpecificActions = true;
                foreach (var conditional in currentPhase.phaseConditionalActions)
                {
                    if (conditional.playerBuildingTrigger != null && playerBuiltBuildings.Contains(conditional.playerBuildingTrigger))
                    {
                        // Trigger conditional response
                        StartCoroutine(ExecuteConditionalAction(conditional));
                        // Remove from set to prevent repeated triggers
                        playerBuiltBuildings.Remove(conditional.playerBuildingTrigger);
                    }
                }
            }

            // Fall back to legacy global conditionals if no phase-specific ones exist
            if (!hasPhaseSpecificActions && config.conditionalActions != null)
            {
                foreach (var conditional in config.conditionalActions)
                {
                    if (conditional.playerBuildingTrigger != null && playerBuiltBuildings.Contains(conditional.playerBuildingTrigger))
                    {
                        // Trigger conditional response
                        StartCoroutine(ExecuteConditionalAction(conditional));
                        // Remove from set to prevent repeated triggers
                        playerBuiltBuildings.Remove(conditional.playerBuildingTrigger);
                    }
                }
            }
        }

        void StartTransition()
        {
            // 3-phase transition: Cleanup → Bonus → Swap
            StartCoroutine(ExecuteTransition());
        }

        IEnumerator ExecuteTransition()
        {
            // Phase 1: Trigger & Cleanup (0.5s)
            CleanupEnemyBuildings();
            SpawnDeathVFX();
            yield return new WaitForSeconds(0.5f);

            // Phase 2: Bonus Selection (paused)
            Time.timeScale = 0;
            yield return StartCoroutine(ShowBonusSelection());
            yield return StartCoroutine(ShowBonusConfirmation());
            Time.timeScale = 1;

            // Phase 3: Swap & Resume (1.5s)
            DeactivateCurrentPhase();
            currentPhaseIndex++;
            yield return StartCoroutine(AnimatePhaseEntry());
            StartCurrentPhase();
        }

        void PreventDamage(object sender, EventArgs e)
        {
            // Override to prevent damage during transition
            // This requires modifying the damage system to allow interception
            // May not be needed if cruiser is positioned far off-screen (>35 units right)
        }

        void CleanupEnemyBuildings()
        {
            // Destroy all enemy buildings - they will be rebuilt in new phase
            // Requires access to enemy building list
        }

        void SpawnDeathVFX()
        {
            // Spawn death explosion using existing CruiserDeathManager logic
        }

        IEnumerator ShowBonusSelection()
        {
            // Display bonus selection UI
            // Requires BonusSelectionPanel implementation
            yield return new WaitForSecondsRealtime(3f); // Wait for player choice
        }

        IEnumerator ShowBonusConfirmation()
        {
            // Show "Bonus Engaged!" message
            yield return new WaitForSecondsRealtime(1.5f);
        }

        void DeactivateCurrentPhase()
        {
            if (currentPhaseIndex > 0)
            {
                // Deactivate current phase cruiser (only for phases > 0, index is currentPhaseIndex - 1)
                int phaseCruiserIndex = currentPhaseIndex - 1;
                if (phaseCruiserIndex < phaseCruisers.Count)
                {
                    var currentPhaseObj = phaseCruisers[phaseCruiserIndex];
                    if (currentPhaseObj != null)
                    {
                        currentPhaseObj.SetActive(false);
                        var cruiser = currentPhaseObj.GetComponentInChildren<Cruiser>();
                        if (cruiser != null)
                        {
                            cruiser.enabled = false;
                        }
                    }
                }
            }
            // Phase 0 (original cruiser) stays active but buildings get cleaned up
        }

        IEnumerator AnimatePhaseEntry()
        {
            int phaseCruiserIndex = currentPhaseIndex - 1;
            var phaseObj = phaseCruisers[phaseCruiserIndex];
            var phase = config.cruiserPhases[currentPhaseIndex];

            if (phase.entryAnimationDuration > 0)
            {
                // Slide in animation
                Vector3 startPos = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 0.5f, Camera.main.nearClipPlane));
                Vector3 endPos = CalculateEnemyPosition();

                float duration = phase.entryAnimationDuration; // Convert int to float for animation
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    phaseObj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                phaseObj.transform.position = endPos;
            }
            else
            {
                // Instant positioning
                phaseObj.transform.position = CalculateEnemyPosition();
            }
        }

        Vector3 CalculateEnemyPosition()
        {
            // Calculate standard enemy cruiser position
            return new Vector3(35f, 0f, 0f); // CRUISER_OFFSET_IN_M from CruiserFactory
        }

        // Building completion tracking for reactive actions
        void OnPlayerBuildingCompleted(IPrefabKey buildingType)
        {
            playerBuiltBuildings.Add(buildingType);
        }

        IEnumerator ExecuteConditionalAction(ConditionalAction conditional)
        {
            yield return new WaitForSeconds(conditional.delayAfterTrigger);

            // Execute slot replacement actions
            if (conditional.slotActions != null && conditional.slotActions.Count > 0)
            {
                foreach (var slotAction in conditional.slotActions)
                {
                    ExecuteSlotAction(slotAction);
                }
            }

            // Show chat if specified
            if (!string.IsNullOrEmpty(conditional.chatKey))
            {
                ShowConditionalChat(conditional.chatKey);
            }
        }

        void ExecuteSlotAction(SlotReplacementAction slotAction)
        {
            if (enemyCruiser == null) return;

            // Find the slot by ID
            foreach (var slot in enemyCruiser.SlotWrapperController.Slots)
            {
                if (slot.Index == slotAction.slotID)
                {
                    // Destroy existing building if any
                    if (slot.Building.Value != null)
                    {
                        slot.Building.Value.Destroy();
                    }

                    // Replace with new building if specified
                    if (slotAction.replacementPrefab != null)
                    {
                        // Get building wrapper prefab
                        var buildingKey = slotAction.replacementPrefab as BuildingKey;
                        if (buildingKey != null)
                        {
                            var buildingWrapper = PrefabFactory.GetBuildingWrapperPrefab(buildingKey);
                            if (buildingWrapper != null)
                            {
                                // Find appropriate slot type
                                var slots = enemyCruiser.SlotAccessor.GetFreeSlots(buildingWrapper.Buildable.SlotSpecification.SlotType);
                                if (slots.Count > 0)
                                {
                                    // Use the specified slot if available, otherwise use first free slot
                                    Slot targetSlot = null;
                                    foreach (var s in slots)
                                    {
                                        if (s.Index == slotAction.slotID)
                                        {
                                            targetSlot = s;
                                            break;
                                        }
                                    }
                                    if (targetSlot == null && slots.Count > 0)
                                    {
                                        targetSlot = slots[0];
                                    }

                                    if (targetSlot != null)
                                    {
                                        enemyCruiser.ConstructBuilding(
                                            buildingWrapper,
                                            targetSlot,
                                            slotAction.ignoreDroneReq,
                                            slotAction.ignoreBuildTime
                                        );
                                    }
                                }
                            }
                        }
                    }

                    break; // Found and processed the slot
                }
            }
        }

        void ShowConditionalChat(string chatKey)
        {
            // Show chat using the extended heckle manager
            var heckleManager = FindObjectOfType<ExtendedNPCHeckleManager>();
            if (heckleManager != null)
            {
                heckleManager.ShowChainBattleChat(chatKey, ChainBattleChat.SpeakerType.EnemyCaptain, 4f);
            }
            else
            {
                Debug.Log($"Showing conditional chat: {chatKey}");
            }
        }

        void ExecuteBuildingAction(SequencePoint.BuildingAction buildingAction)
        {
            if (enemyCruiser == null) return;

            // Find the target slot
            foreach (var slot in enemyCruiser.SlotWrapperController.Slots)
            {
                if (slot.Index == buildingAction.SlotID)
                {
                    // Destroy existing building if any
                    if (slot.Building.Value != null)
                    {
                        slot.Building.Value.Destroy();
                    }

                    if (buildingAction.Operation == SequencePoint.BuildingAction.BuildingOp.Add)
                    {
                        // Build new building
                        var buildingKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingAction.PrefabKeyName);
                        var buildingWrapper = PrefabFactory.GetBuildingWrapperPrefab(buildingKey);
                        if (buildingWrapper != null)
                        {
                            // Find appropriate slot type
                            var slots = enemyCruiser.SlotAccessor.GetFreeSlots(buildingWrapper.Buildable.SlotSpecification.SlotType);
                            if (slots.Count > 0)
                            {
                                // Use the specified slot if available and compatible
                                Slot targetSlot = null;
                                foreach (var s in slots)
                                {
                                    if (s.Index == buildingAction.SlotID)
                                    {
                                        targetSlot = s;
                                        break;
                                    }
                                }

                                if (targetSlot != null)
                                {
                                    // Build the building
                                    enemyCruiser.ConstructBuilding(buildingWrapper, targetSlot, buildingAction.IgnoreDroneReq, buildingAction.IgnoreBuildTime);
                                }
                            }
                        }
                    }
                    // For destroy operation, we already destroyed above
                    break;
                }
            }
        }

        void ExecuteUnitAction(SequencePoint.UnitAction unitAction)
        {
            // Check factory requirement (0 = no requirement)
            if ((int)unitAction.RequiredFactory != 0)
            {
                // Check if the required factory exists on the enemy cruiser
                bool factoryExists = false;
                if (enemyCruiser != null && enemyCruiser.SlotWrapperController != null)
                {
                    foreach (var slot in enemyCruiser.SlotWrapperController.Slots)
                    {
                        if (slot.Building.Value != null &&
                            slot.Building.Value.keyName == unitAction.RequiredFactory.ToString())
                        {
                            factoryExists = true;
                            break;
                        }
                    }
                }

                if (!factoryExists)
                {
                    // Factory not found, skip unit spawning
                    Debug.Log($"Skipping unit spawn: required factory {unitAction.RequiredFactory} not found");
                    return;
                }
            }

            // Spawn the units (using existing battle sequencer logic)
            var battleSequencer = FindObjectOfType<BattleSequencer>();
            if (battleSequencer != null)
            {
                // Use the battle sequencer's SpawnUnit method
                battleSequencer.SpawnUnit(unitAction.PrefabKeyName, unitAction.Postion, enemyCruiser);
            }
        }
    }
}
