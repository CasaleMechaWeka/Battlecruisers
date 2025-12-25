using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Models.PrefabKeys; // For IPrefabKey
using BattleCruisers.Data.Static.LevelLoot; // For Loot
using System.Linq; // For ToList, FirstOrDefault
using BattleCruisers.UI.ScreensScene.ShopScreen; // For BodykitData
using BattleCruisers.Buildables.Boost.GlobalProviders; // For BoostType
using BattleCruisers.Scenes.BattleScene; // For SequencePoint
using System.Reflection; // For reflection to get static properties
using BattleCruisers.UI.Sound; // For SoundKeyPair
using BattleCruisers.Utils; // For PrefabKeyName and StaticPrefabKeyHelper
using BattleCruisers.Buildables; // For Faction
using BattleCruisers.Cruisers; // For Cruiser.BoostStats
using BattleCruisers.Utils.Fetchers; // For PrefabFactory
using BattleCruisers.Utils.Localisation; // For LocTableCache

public class ChainBattleEditorWindow : EditorWindow
{
    private ChainBattleConfiguration currentConfig;
    private Vector2 scrollPos;
    private int selectedTab = 0;
    private string[] tabs = { "Basic", "Phases", "Testing" };

    [MenuItem("Tools/ChainBattle Editor")]
    [MenuItem("Window/ChainBattle Editor")] // Also available in Window menu
    static void Init()
    {
        ChainBattleEditorWindow window = (ChainBattleEditorWindow)EditorWindow.GetWindow(typeof(ChainBattleEditorWindow));
        window.titleContent = new GUIContent("ChainBattle Editor");
        window.Show();
    }

    void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabs);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        switch (selectedTab)
        {
            case 0: DrawBasicTab(); break;
            case 1: DrawPhasesTab(); break;
            case 2: DrawTestingTab(); break;
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawBasicTab()
    {
        EditorGUILayout.LabelField("ChainBattle Editor", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Create sequential boss battles with multiple enemy phases.\n\n⚠️ Editor UI is in development. Use 'Edit in Inspector' for full control of phases and chat keys.", MessageType.Warning);
        EditorGUILayout.Space();

        // Current config field
        currentConfig = (ChainBattleConfiguration)EditorGUILayout.ObjectField(
            "Current Config", currentConfig, typeof(ChainBattleConfiguration), false);

        if (currentConfig != null && GUILayout.Button("Edit in Inspector"))
        {
            Selection.activeObject = currentConfig;
        }

        EditorGUILayout.Space();

        // Load existing config button
        if (GUILayout.Button("Load Existing ChainBattle"))
        {
            string path = EditorUtility.OpenFilePanel("Select ChainBattle Configuration", "Assets/Resources/ChainBattles", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
                ChainBattleConfiguration loadedConfig = AssetDatabase.LoadAssetAtPath<ChainBattleConfiguration>(path);
                if (loadedConfig != null)
                {
                    currentConfig = loadedConfig;
                    EditorUtility.SetDirty(currentConfig);
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Failed to load ChainBattle configuration from selected file.", "OK");
                }
            }
        }

        // Create new config button
        if (GUILayout.Button("Create New ChainBattle"))
        {
            currentConfig = CreateInstance<ChainBattleConfiguration>();
            currentConfig.name = "New ChainBattle";
            currentConfig.levelNameKey = "CHAINBATTLE_NAME";
            currentConfig.levelNumber = GetNextAvailableLevelNumber();
            currentConfig.musicKeys = GetMusicFromIndex(1); // Default to Bobby
            currentConfig.skyMaterialName = SkyMaterials.Dusk; // Default sky
            currentConfig.captainExoKey = GetCaptainFromIndex(1); // Default captain
            currentConfig.cruiserPhases = new List<CruiserPhase> { new CruiserPhase() };
            currentConfig.conditionalActions = new List<ConditionalAction>();
        }

        // Create Fei demo button
        if (GUILayout.Button("Create Fei Demo ChainBattle (Level 32)"))
        {
            CreateFeiDemoChainBattle();
        }

        EditorGUILayout.Space();

        if (currentConfig != null)
        {
            // Basic properties
            EditorGUILayout.LabelField("Basic Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            var newLevelNumber = EditorGUILayout.IntField("Level Number", currentConfig.levelNumber);
            var newLevelNameKey = EditorGUILayout.TextField("Level Name Key", currentConfig.levelNameKey);
            var newPlayerTalksFirst = EditorGUILayout.Toggle("Player Talks First", currentConfig.playerTalksFirst);

            // Music selection
            EditorGUILayout.LabelField("Music", EditorStyles.boldLabel);
            int musicIndex = GetMusicIndex(currentConfig.musicKeys);
            int newMusicIndex = EditorGUILayout.Popup("Background Music", musicIndex, GetMusicOptions());
            if (newMusicIndex != musicIndex)
            {
                currentConfig.musicKeys = GetMusicFromIndex(newMusicIndex);
                EditorUtility.SetDirty(currentConfig);
            }

            // Sky material selection
            EditorGUILayout.LabelField("Sky Material", EditorStyles.boldLabel);
            int skyIndex = GetSkyIndex(currentConfig.skyMaterialName);
            int newSkyIndex = EditorGUILayout.Popup("Sky Material", skyIndex, GetSkyOptions());
            if (newSkyIndex != skyIndex)
            {
                currentConfig.skyMaterialName = GetSkyFromIndex(newSkyIndex);
                EditorUtility.SetDirty(currentConfig);
            }

            // Captain selection
            EditorGUILayout.LabelField("Captain", EditorStyles.boldLabel);
            string[] captainOptions = GetCaptainOptions();
            int captainIndex = GetCaptainIndex(currentConfig.captainExoKey as CaptainExoKey);
            int newCaptainIndex = EditorGUILayout.Popup("Captain Exo", captainIndex, captainOptions);
            if (newCaptainIndex != captainIndex)
            {
                currentConfig.captainExoKey = GetCaptainFromIndex(newCaptainIndex);
                EditorUtility.SetDirty(currentConfig);
            }
            EditorGUILayout.HelpBox("Captain remains the same throughout all phases. Level Name Key should match the captain name.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                // Validate level number is unique
                if (newLevelNumber != currentConfig.levelNumber)
                {
                    var existingBattles = StaticData.ChainBattles;
                    bool isUnique = true;
                    foreach (var battle in existingBattles)
                    {
                        if (battle != currentConfig && battle.levelNumber == newLevelNumber)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (!isUnique)
                    {
                        EditorUtility.DisplayDialog("Error", "Level number must be unique!", "OK");
                        newLevelNumber = currentConfig.levelNumber;
                    }
                }

                currentConfig.levelNumber = newLevelNumber;
                currentConfig.levelNameKey = newLevelNameKey;
                currentConfig.playerTalksFirst = newPlayerTalksFirst;
                EditorUtility.SetDirty(currentConfig);
            }
            EditorGUILayout.HelpBox("Level Name Key = The text that appears in the level selection menu (e.g., 'ChainBattle: Fei').\n\nBoth level number AND name key are required:\n• Level Number (32) = Used for game logic, trash talk, and progression\n• Name Key = The display name players see in the UI\n\nEven though there's only one 'level 32', the name key allows for descriptive titles like 'ChainBattle: Final Boss'.", MessageType.Info);

            // Story Variables (Trash Talk & Appraisal)
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Story Variables", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("These appear in the pre-battle trash talk screen and post-battle drone appraisal.", MessageType.Info);

            // Generate default story keys if needed
            if (currentConfig.customChats == null || !currentConfig.customChats.Any(c => c.chatKey.Contains("PlayerText")))
            {
                if (GUILayout.Button("Generate Default Story Keys", GUILayout.Width(180)))
                {
                    GenerateDefaultStoryKeys();
                }
            }

            // Display existing story chats
            if (currentConfig.customChats != null)
            {
                var storyChats = currentConfig.customChats.Where(c => c.chatKey.Contains("PlayerText") || c.chatKey.Contains("EnemyText") || c.chatKey.Contains("DroneText")).ToList();
                foreach (var chat in storyChats)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    var chatType = chat.chatKey.Contains("PlayerText") ? "Player Trash Talk" :
                                   chat.chatKey.Contains("EnemyText") ? "Enemy Trash Talk" :
                                   chat.chatKey.Contains("DroneText") ? "Drone Appraisal" : "Story Chat";

                    EditorGUILayout.LabelField(chatType, EditorStyles.miniBoldLabel);
                    EditorGUILayout.LabelField($"Key: {chat.chatKey}", EditorStyles.miniLabel);
                    chat.speaker = (ChainBattleChat.SpeakerType)EditorGUILayout.EnumPopup("Speaker", chat.speaker);
                    chat.displayDuration = EditorGUILayout.Slider("Duration", chat.displayDuration, 1f, 10f);
                    EditorGUILayout.LabelField("English Text:");
                    chat.englishText = EditorGUILayout.TextArea(chat.englishText, GUILayout.Height(40));
                    EditorGUILayout.EndVertical();
                }
            }

            // Auto-generate default localization keys
            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Default Localization Keys", GUILayout.Width(220)))
            {
                GenerateDefaultLocalizationKeys();
            }
            EditorGUILayout.HelpBox("Creates the 5 essential localization keys for this level: PlayerName, PlayerText, EnemyText, DroneText, PlayerChat1", MessageType.Info);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("ChainBattle Content", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use the Phases tab to edit cruiser phases, Chat tab for conversations, and Reactive tab for conditional actions.", MessageType.Info);

            EditorGUILayout.Space();

            // Edit legacy dialog keys (for backwards compatibility)
            EditorGUILayout.LabelField("Legacy Dialog Keys", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Legacy support for old dialog keys. New ChainBattles should use the Chat system in phases and reactive actions instead.", MessageType.Info);

            for (int i = 0; i < currentConfig.dialogKeys.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var newValue = EditorGUILayout.TextField($"Dialog Key {i + 1}", currentConfig.dialogKeys[i]);
                if (newValue != currentConfig.dialogKeys[i])
                {
                    currentConfig.dialogKeys[i] = newValue;
                    EditorUtility.SetDirty(currentConfig);
                }

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    currentConfig.dialogKeys.RemoveAt(i);
                    EditorUtility.SetDirty(currentConfig);
                    break; // Exit loop since we modified the collection
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Dialog Key"))
            {
                currentConfig.dialogKeys.Add("");
                EditorUtility.SetDirty(currentConfig);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Completion loot is automatically calculated based on level number, similar to regular campaign levels.", MessageType.Info);

            EditorGUILayout.Space();

            // Save button
            if (GUILayout.Button("Save ChainBattle Asset"))
            {
                SaveChainBattleAsset();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tips for Great ChainBattles:", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox(
                "• 2-4 phases work best\n" +
                "• Final phase should be most challenging\n" +
                "• Use different hull types for variety\n" +
                "• Entry animations: 0 = instant, 2-4 = slide-in\n" +
                "• Dialog keys should follow pattern: CHAINBATTLE_[NAME]_[NUMBER]",
                MessageType.Info);

            EditorGUILayout.LabelField("Note: Save the asset in Assets/Resources/ChainBattles/", EditorStyles.helpBox);
        }
        else
        {
            EditorGUILayout.HelpBox("Create a new ChainBattle or select an existing one to edit.", MessageType.Info);
        }
    }

    void DrawPhasesTab()
    {
        EditorGUILayout.LabelField("Cruiser Phases", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Define each enemy cruiser phase. Only the last phase should be marked 'Final Phase'.", MessageType.Info);

        if (currentConfig.cruiserPhases == null) currentConfig.cruiserPhases = new List<CruiserPhase>();

        string[] hullOptions = GetHullOptions();
        string[] captainOptions = GetCaptainOptions();

        for (int phaseIndex = 0; phaseIndex < currentConfig.cruiserPhases.Count; phaseIndex++)
        {
            CruiserPhase currentPhase = currentConfig.cruiserPhases[phaseIndex];
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"Phase {phaseIndex + 1}", EditorStyles.boldLabel);

            // Hull selection dropdown
            int hullIndex = GetHullIndex(currentPhase.hullKey as HullKey);
            int newHullIndex = EditorGUILayout.Popup("Hull Prefab", hullIndex, hullOptions);
            if (newHullIndex != hullIndex)
            {
                currentPhase.hullKey = GetHullFromIndex(newHullIndex);
                EditorUtility.SetDirty(currentConfig);
            }

            // Display slot information for this hull
            DisplayHullSlotInfo(currentPhase.hullKey as HullKey);

            // Bodykit selection (single selection with None default)
            EditorGUILayout.LabelField("Bodykit", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Select one bodykit for this phase cruiser (optional cosmetic upgrade).", MessageType.Info);

            // Get compatible bodykits for selected hull
            var compatibleBodykits = GetCompatibleBodykits(currentPhase.hullKey as HullKey);
            var bodykitOptions = new List<string> { "None (Default)" };
            bodykitOptions.AddRange(compatibleBodykits.Select(b => $"{LocTableCache.CommonTable.GetString(b.NameStringKeyBase)} (ID: {b.Index})"));

            int currentBodykitIndex = 0; // Default to None
            if (currentPhase.bodykits != null && currentPhase.bodykits.Length > 0)
            {
                var selectedBodykit = currentPhase.bodykits[0];
                var bodykitIndex = compatibleBodykits.FindIndex(b => b.Index == selectedBodykit.Index);
                if (bodykitIndex >= 0)
                {
                    currentBodykitIndex = bodykitIndex + 1; // +1 because "None" is at index 0
                }
            }

            int newBodykitIndex = EditorGUILayout.Popup("Bodykit", currentBodykitIndex, bodykitOptions.ToArray());
            if (newBodykitIndex != currentBodykitIndex)
            {
                if (newBodykitIndex == 0)
                {
                    // None selected
                    currentPhase.bodykits = new BodykitData[0];
                }
                else
                {
                    // Specific bodykit selected
                    currentPhase.bodykits = new BodykitData[] { compatibleBodykits[newBodykitIndex - 1] };
                }
                EditorUtility.SetDirty(currentConfig);
            }

            currentPhase.isFinalPhase = EditorGUILayout.Toggle("Is Final Phase", currentPhase.isFinalPhase);
            currentPhase.entryAnimationDuration = EditorGUILayout.IntField("Entry Animation Duration (seconds)", currentPhase.entryAnimationDuration);
            
            // Phase Dialog
            EditorGUILayout.LabelField("Phase Dialog", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Dialog shown when this phase starts. Add player and enemy responses for natural conversation flow.", MessageType.Info);

            // Default enemy chat
            if (currentPhase.phaseStartChats == null || currentPhase.phaseStartChats.Count == 0)
            {
                currentPhase.phaseStartChats = new List<ChainBattleChat>();
            }

            // Ensure we have at least one enemy chat
            if (!currentPhase.phaseStartChats.Any(c => c.speaker == ChainBattleChat.SpeakerType.EnemyCaptain))
            {
                currentPhase.phaseStartChats.Insert(0, new ChainBattleChat {
                    chatKey = $"level{currentConfig.levelNumber}/Phase{phaseIndex + 1}EnemyIntro",
                    speaker = ChainBattleChat.SpeakerType.EnemyCaptain,
                    displayDuration = 4f
                });
            }

            for (int chatIndex = 0; chatIndex < currentPhase.phaseStartChats.Count; chatIndex++)
            {
                ChainBattleChat currentChat = currentPhase.phaseStartChats[chatIndex];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField($"{currentChat.speaker} Chat {chatIndex + 1}", EditorStyles.miniBoldLabel);

                DrawChatKeySelector(ref currentChat.chatKey);
                currentChat.displayDuration = EditorGUILayout.Slider("Display Duration", currentChat.displayDuration, 1f, 10f);
                EditorGUILayout.LabelField("English Text (for translation context):");
                currentChat.englishText = EditorGUILayout.TextArea(currentChat.englishText, GUILayout.Height(60));

                if (GUILayout.Button("Remove Chat", GUILayout.Width(120)))
                {
                    currentPhase.phaseStartChats.RemoveAt(chatIndex);
                    EditorUtility.SetDirty(currentConfig);
                    break;
                }
                EditorGUILayout.EndVertical();
            }

            // Calculate next available chat numbers for this phase
            int enemyChatCount = currentPhase.phaseStartChats.Count(c => c.speaker == ChainBattleChat.SpeakerType.EnemyCaptain);
            int playerChatCount = currentPhase.phaseStartChats.Count(c => c.speaker == ChainBattleChat.SpeakerType.PlayerCaptain);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Enemy Chat", GUILayout.Width(120)))
            {
                currentPhase.phaseStartChats.Add(new ChainBattleChat {
                    chatKey = $"level{currentConfig.levelNumber}/Phase{phaseIndex + 1}Enemy{enemyChatCount + 1}",
                    speaker = ChainBattleChat.SpeakerType.EnemyCaptain,
                    displayDuration = 4f
                });
                EditorUtility.SetDirty(currentConfig);
            }
            if (GUILayout.Button("Add Player Chat", GUILayout.Width(120)))
            {
                currentPhase.phaseStartChats.Add(new ChainBattleChat {
                    chatKey = $"level{currentConfig.levelNumber}/Phase{phaseIndex + 1}Player{playerChatCount + 1}",
                    speaker = ChainBattleChat.SpeakerType.PlayerCaptain,
                    displayDuration = 3f
                });
                EditorUtility.SetDirty(currentConfig);
            }
            EditorGUILayout.EndHorizontal();

            // Phase Reactive Behaviors
            EditorGUILayout.LabelField("Phase Reactive Behaviors", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Enemy reactions to player actions during this phase (e.g., building an air factory triggers countermeasures).", MessageType.Info);

            if (currentPhase.phaseConditionalActions == null) currentPhase.phaseConditionalActions = new ConditionalAction[0];

            var reactiveList = new List<ConditionalAction>(currentPhase.phaseConditionalActions);
            for (int reactionIndex = 0; reactionIndex < reactiveList.Count; reactionIndex++)
            {
                var conditional = reactiveList[reactionIndex];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField($"Reaction {reactionIndex + 1}", EditorStyles.miniBoldLabel);

                // Trigger
                EditorGUILayout.LabelField("Trigger: Player builds...", EditorStyles.miniBoldLabel);
                var triggerBuildingName = conditional.playerBuildingTrigger != null ?
                    conditional.playerBuildingTrigger.PrefabName : "None";
                EditorGUILayout.LabelField($"Building: {triggerBuildingName}", EditorStyles.miniLabel);

                // TODO: Add building selection dropdown when we implement it

                // Response
                EditorGUILayout.LabelField("Response", EditorStyles.miniBoldLabel);
                conditional.delayAfterTrigger = EditorGUILayout.Slider("Wait Time (seconds)", conditional.delayAfterTrigger, 0f, 30f);

                // Slot actions
                if (conditional.slotActions != null)
                {
                    foreach (var slotAction in conditional.slotActions)
                    {
                        EditorGUILayout.LabelField($"Slot {slotAction.slotID}: Replace with {slotAction.replacementPrefab?.PrefabName ?? "destroy"}", EditorStyles.miniLabel);
                    }
                }

                // Chat
                if (!string.IsNullOrEmpty(conditional.chatKey))
                {
                    DrawChatKeySelector(ref conditional.chatKey);
                }

                if (GUILayout.Button("Remove Reaction", GUILayout.Width(130)))
                {
                    reactiveList.RemoveAt(reactionIndex);
                    currentPhase.phaseConditionalActions = reactiveList.ToArray();
                    EditorUtility.SetDirty(currentConfig);
                    break;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Reactive Behavior", GUILayout.Width(160)))
            {
                reactiveList.Add(new ConditionalAction());
                currentPhase.phaseConditionalActions = reactiveList.ToArray();
                EditorUtility.SetDirty(currentConfig);
            }
            
            // Initial Buildings - Slot-based approach
            EditorGUILayout.LabelField("Initial Buildings", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Configure what buildings are placed in each slot on the enemy cruiser when this phase starts.", MessageType.Info);

            if (currentPhase.hullKey != null)
            {
                var slotCounts = GetCruiserSlotCounts(currentPhase.hullKey);
                if (slotCounts.Count > 0)
                {
                    // Initialize building actions if needed
                    if (currentPhase.initialBuildings == null) currentPhase.initialBuildings = new SequencePoint.BuildingAction[0];

                    // Group slots by type for better organization
                    var allSlots = new List<(byte slotId, string slotType)>();
                    var cruiserPrefab = PrefabFactory.GetCruiserPrefab(currentPhase.hullKey as HullKey);
                    if (cruiserPrefab != null)
                    {
                        var cruiser = cruiserPrefab.GetComponent<Cruiser>();
                        if (cruiser != null && cruiser.SlotWrapperController != null)
                        {
                            var slots = cruiser.SlotWrapperController.Slots;
                            for (byte slotIndex = 0; slotIndex < slots.Count; slotIndex++)
                            {
                                var slotType = GetSlotTypeName(slots[slotIndex].Type);
                                allSlots.Add((slotIndex, slotType));
                            }
                        }
                    }

                    // Display slots grouped by type
                    var slotGroups = allSlots.GroupBy(s => s.slotType).OrderBy(g => g.Key);

                    foreach (var slotGroup in slotGroups)
                    {
                        EditorGUILayout.LabelField($"{slotGroup.Key} Slots", EditorStyles.miniBoldLabel);

                        foreach (var slot in slotGroup.OrderBy(s => s.slotId))
                        {
                            EditorGUILayout.BeginVertical(GUI.skin.box);
                            EditorGUILayout.LabelField($"Slot {slot.slotId} ({slot.slotType})", EditorStyles.miniBoldLabel);

                            // Find existing building action for this slot
                            var existingAction = currentPhase.initialBuildings.FirstOrDefault(b => b.SlotID == slot.slotId && b.Operation == SequencePoint.BuildingAction.BuildingOp.Add);

                            if (existingAction != null)
                            {
                                // Show current building with option to change or remove
                                var buildingName = GetBuildingDisplayName(existingAction.PrefabKeyName);
                                EditorGUILayout.LabelField($"Current: {buildingName}", EditorStyles.miniLabel);

                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("Change Building", GUILayout.Width(120)))
                                {
                                    // Show building selection popup for this slot
                                    ShowBuildingSelectionForSlot(slot.slotId, slot.slotType, currentPhase);
                                }
                                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                                {
                                    RemoveBuildingFromSlot(slot.slotId, currentPhase);
                                }
                                EditorGUILayout.EndHorizontal();

                                // Show build flags
                                existingAction.IgnoreDroneReq = EditorGUILayout.Toggle("Ignore Drone Requirement", existingAction.IgnoreDroneReq);
                                existingAction.IgnoreBuildTime = EditorGUILayout.Toggle("Ignore Build Time", existingAction.IgnoreBuildTime);
                            }
                            else
                            {
                                // Show option to add building
                                if (GUILayout.Button("Add Building", GUILayout.Width(100)))
                                {
                                    ShowBuildingSelectionForSlot(slot.slotId, slot.slotType, currentPhase);
                                }
                            }

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space(2);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Unable to load slot information for this hull.", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select a hull first to configure initial buildings.", MessageType.Info);
            }
            
            // Initial Units
            EditorGUILayout.LabelField("Initial Units", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Units to spawn when this phase starts.", MessageType.Info);
            if (currentPhase.initialUnits == null) currentPhase.initialUnits = new SequencePoint.UnitAction[0];
            
            var unitList = new List<SequencePoint.UnitAction>(currentPhase.initialUnits);
            for (int j = 0; j < unitList.Count; j++)
            {
                var unitAction = unitList[j];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField($"Unit {j + 1}", EditorStyles.miniBoldLabel);
                
                // Unit selection
                int unitIndex = GetUnitIndex(unitAction.PrefabKeyName);
                int newUnitIndex = EditorGUILayout.Popup("Unit", unitIndex, GetUnitOptions());
                if (newUnitIndex != unitIndex)
                {
                    unitAction.PrefabKeyName = GetUnitFromIndex(newUnitIndex);
                    EditorUtility.SetDirty(currentConfig);
                }
                
                unitAction.Postion = EditorGUILayout.Vector2Field("Position", unitAction.Postion);
                unitAction.SpawnArea = EditorGUILayout.Vector2Field("Spawn Area", unitAction.SpawnArea);
                unitAction.Amount = (byte)EditorGUILayout.IntSlider("Amount", unitAction.Amount, 1, 255);

                // Factory requirement
                EditorGUILayout.LabelField("Factory Requirement:", EditorStyles.miniBoldLabel);
                EditorGUILayout.HelpBox("Aircraft need AirFactory, ships need NavalFactory. Select first option for no requirement.", MessageType.Info);
                unitAction.RequiredFactory = (PrefabKeyName)EditorGUILayout.EnumPopup("Required Factory", unitAction.RequiredFactory);
                
                if (GUILayout.Button("Remove Unit", GUILayout.Width(150)))
                {
                    unitList.RemoveAt(j);
                    currentPhase.initialUnits = unitList.ToArray();
                    EditorUtility.SetDirty(currentConfig);
                    break;
                }
                EditorGUILayout.EndVertical();
            }
            
            if (GUILayout.Button("Add Unit", GUILayout.Width(150)))
            {
                unitList.Add(new SequencePoint.UnitAction { Amount = 1 });
                currentPhase.initialUnits = unitList.ToArray();
                EditorUtility.SetDirty(currentConfig);
            }

            // Phase Bonuses
            EditorGUILayout.LabelField("Phase Bonuses", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Stat bonuses applied to this phase's cruiser (stacks with base stats).", MessageType.Info);
            if (currentPhase.phaseBonuses == null) currentPhase.phaseBonuses = new Cruiser.BoostStats[0];

            var bonusesList = new List<Cruiser.BoostStats>(currentPhase.phaseBonuses);
            for (int j = 0; j < bonusesList.Count; j++)
            {
                var bonus = bonusesList[j];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField($"Bonus {j + 1}", EditorStyles.miniBoldLabel);

                bonus.boostType = (BoostType)EditorGUILayout.EnumPopup("Boost Type", bonus.boostType);
                bonus.boostAmount = EditorGUILayout.FloatField("Boost Amount", bonus.boostAmount);

                if (GUILayout.Button("Remove Bonus", GUILayout.Width(120)))
                {
                    bonusesList.RemoveAt(j);
                    currentPhase.phaseBonuses = bonusesList.ToArray();
                    EditorUtility.SetDirty(currentConfig);
                    break;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Bonus", GUILayout.Width(120)))
            {
                bonusesList.Add(new Cruiser.BoostStats { boostType = BoostType.HealthAllBuilding, boostAmount = 1.0f });
                currentPhase.phaseBonuses = bonusesList.ToArray();
                EditorUtility.SetDirty(currentConfig);
            }

            // Sequence Points
            EditorGUILayout.LabelField("Sequence Points", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Timed sequences of actions for this phase. Actions execute in order based on DelayMS.", MessageType.Info);
            if (currentPhase.phaseSequencePoints == null) currentPhase.phaseSequencePoints = new SequencePoint[0];
            
            var sequenceList = new List<SequencePoint>(currentPhase.phaseSequencePoints);
            for (int j = 0; j < sequenceList.Count; j++)
            {
                var seqPoint = sequenceList[j];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField($"Sequence Point {j + 1}", EditorStyles.miniBoldLabel);
                
                seqPoint.DelayMS = EditorGUILayout.IntField("Delay (ms)", seqPoint.DelayMS);
                seqPoint.Faction = (Faction)EditorGUILayout.EnumPopup("Faction", seqPoint.Faction);
                
                // Building Actions
                EditorGUILayout.LabelField("Building Actions", EditorStyles.miniBoldLabel);
                if (seqPoint.BuildingActions == null) seqPoint.BuildingActions = new List<SequencePoint.BuildingAction>();
                var buildingActionsList = seqPoint.BuildingActions;
                for (int k = 0; k < buildingActionsList.Count; k++)
                {
                    var buildingAction = buildingActionsList[k];
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    buildingAction.Operation = (SequencePoint.BuildingAction.BuildingOp)EditorGUILayout.EnumPopup("Operation", buildingAction.Operation);
                    if (buildingAction.Operation == SequencePoint.BuildingAction.BuildingOp.Add)
                    {
                        int buildingIndex = GetBuildingIndex(buildingAction.PrefabKeyName);
                        int newBuildingIndex = EditorGUILayout.Popup("Building", buildingIndex, GetBuildingOptions());
                        if (newBuildingIndex != buildingIndex)
                        {
                            buildingAction.PrefabKeyName = GetBuildingFromIndex(newBuildingIndex);
                            EditorUtility.SetDirty(currentConfig);
                        }
                    }
                    buildingAction.SlotID = (byte)EditorGUILayout.IntField("Slot ID", buildingAction.SlotID);
                    if (buildingAction.Operation == SequencePoint.BuildingAction.BuildingOp.Add)
                    {
                        buildingAction.IgnoreDroneReq = EditorGUILayout.Toggle("Ignore Drone Req", buildingAction.IgnoreDroneReq);
                        buildingAction.IgnoreBuildTime = EditorGUILayout.Toggle("Ignore Build Time", buildingAction.IgnoreBuildTime);
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        buildingActionsList.RemoveAt(k);
                        EditorUtility.SetDirty(currentConfig);
                        break;
                    }
                    EditorGUILayout.EndVertical();
                }
                if (GUILayout.Button("Add Building Action", GUILayout.Width(150)))
                {
                    buildingActionsList.Add(new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add });
                    EditorUtility.SetDirty(currentConfig);
                }
                
                // Boost Actions
                EditorGUILayout.LabelField("Boost Actions", EditorStyles.miniBoldLabel);
                if (seqPoint.BoostActions == null) seqPoint.BoostActions = new List<SequencePoint.BoostAction>();
                var boostActionsList = seqPoint.BoostActions;
                for (int k = 0; k < boostActionsList.Count; k++)
                {
                    var boostAction = boostActionsList[k];
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    boostAction.Operation = (SequencePoint.BoostAction.BoostOp)EditorGUILayout.EnumPopup("Operation", boostAction.Operation);
                    boostAction.BoostType = (BoostType)EditorGUILayout.EnumPopup("Boost Type", boostAction.BoostType);
                    if (boostAction.Operation != SequencePoint.BoostAction.BoostOp.Remove)
                    {
                        boostAction.BoostAmount = EditorGUILayout.FloatField("Boost Amount", boostAction.BoostAmount);
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        boostActionsList.RemoveAt(k);
                        EditorUtility.SetDirty(currentConfig);
                        break;
                    }
                    EditorGUILayout.EndVertical();
                }
                if (GUILayout.Button("Add Boost Action", GUILayout.Width(150)))
                {
                    boostActionsList.Add(new SequencePoint.BoostAction { Operation = SequencePoint.BoostAction.BoostOp.Add });
                    EditorUtility.SetDirty(currentConfig);
                }
                
                // Unit Actions
                EditorGUILayout.LabelField("Unit Actions", EditorStyles.miniBoldLabel);
                if (seqPoint.UnitActions == null) seqPoint.UnitActions = new List<SequencePoint.UnitAction>();
                var unitActionsList = seqPoint.UnitActions;
                for (int k = 0; k < unitActionsList.Count; k++)
                {
                    var unitAction = unitActionsList[k];
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    int unitIndex = GetUnitIndex(unitAction.PrefabKeyName);
                    int newUnitIndex = EditorGUILayout.Popup("Unit", unitIndex, GetUnitOptions());
                    if (newUnitIndex != unitIndex)
                    {
                        unitAction.PrefabKeyName = GetUnitFromIndex(newUnitIndex);
                        EditorUtility.SetDirty(currentConfig);
                    }
                    unitAction.Postion = EditorGUILayout.Vector2Field("Position", unitAction.Postion);
                    unitAction.SpawnArea = EditorGUILayout.Vector2Field("Spawn Area", unitAction.SpawnArea);
                    unitAction.Amount = (byte)EditorGUILayout.IntSlider("Amount", unitAction.Amount, 1, 255);
                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        unitActionsList.RemoveAt(k);
                        EditorUtility.SetDirty(currentConfig);
                        break;
                    }
                    EditorGUILayout.EndVertical();
                }
                if (GUILayout.Button("Add Unit Action", GUILayout.Width(150)))
                {
                    unitActionsList.Add(new SequencePoint.UnitAction { Amount = 1 });
                    EditorUtility.SetDirty(currentConfig);
                }
                
                // Script Call Actions (simplified - UnityEvents are complex to edit in custom editor)
                EditorGUILayout.LabelField("Script Call Actions", EditorStyles.miniBoldLabel);
                EditorGUILayout.HelpBox("Script call actions (UnityEvents) should be configured in the Inspector after selecting the asset.", MessageType.None);
                
                if (GUILayout.Button("Remove Sequence Point", GUILayout.Width(180)))
                {
                    sequenceList.RemoveAt(j);
                    currentPhase.phaseSequencePoints = sequenceList.ToArray();
                    EditorUtility.SetDirty(currentConfig);
                    break;
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            if (GUILayout.Button("Add Sequence Point", GUILayout.Width(180)))
            {
                sequenceList.Add(new SequencePoint { DelayMS = 0, Faction = Faction.Reds });
                currentPhase.phaseSequencePoints = sequenceList.ToArray();
                EditorUtility.SetDirty(currentConfig);
            }

            if (GUILayout.Button("Remove Phase"))
            {
                currentConfig.cruiserPhases.RemoveAt(phaseIndex);
                EditorUtility.SetDirty(currentConfig);
                break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Phase"))
        {
            currentConfig.cruiserPhases.Add(new CruiserPhase());
            EditorUtility.SetDirty(currentConfig);
        }
    }

    void DrawChatTab()
    {
        EditorGUILayout.LabelField("Custom Dialogs", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Define custom chats using localization keys. Use format: level{number}/{chatName} (e.g., level999/PlayerChat1)", MessageType.Info);

        if (currentConfig.customChats == null) currentConfig.customChats = new List<ChainBattleChat>();

        for (int i = 0; i < currentConfig.customChats.Count; i++)
        {
            ChainBattleChat chat = currentConfig.customChats[i];
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"Chat {i + 1}", EditorStyles.boldLabel);

                DrawChatKeySelector(ref chat.chatKey);
            chat.speaker = (ChainBattleChat.SpeakerType)EditorGUILayout.EnumPopup("Speaker", chat.speaker);
            chat.displayDuration = EditorGUILayout.Slider("Display Duration", chat.displayDuration, 1f, 10f);

            if (GUILayout.Button("Remove Chat"))
            {
                currentConfig.customChats.RemoveAt(i);
                EditorUtility.SetDirty(currentConfig);
                break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        // Quick-add buttons for common chat types
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New Enemy Chat"))
        {
            AddNewChatKey("EnemyChat");
        }
        if (GUILayout.Button("New Player Chat"))
        {
            AddNewChatKey("PlayerChat");
        }
        if (GUILayout.Button("New Reaction Chat"))
        {
            AddNewChatKey("EnemyReaction");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New Drone Chat"))
        {
            AddNewChatKey("Drone");
        }
        if (GUILayout.Button("Add Custom Chat"))
        {
            currentConfig.customChats.Add(new ChainBattleChat { chatKey = $"level{currentConfig.levelNumber}/NewChat{currentConfig.customChats.Count + 1}" });
            EditorUtility.SetDirty(currentConfig);
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawReactiveTab()
    {
        EditorGUILayout.LabelField("Reactive Behaviors (Conditional Actions)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Define enemy reactions to player building specific structures. When player builds X, wait Y seconds, then perform slot actions and show chat.", MessageType.Info);

        if (currentConfig.conditionalActions == null) currentConfig.conditionalActions = new List<ConditionalAction>();

        for (int i = 0; i < currentConfig.conditionalActions.Count; i++)
        {
            ConditionalAction conditional = currentConfig.conditionalActions[i];
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"Conditional Action {i + 1}", EditorStyles.boldLabel);

            // Trigger: Player building to watch for
            EditorGUILayout.LabelField("Trigger", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Watch for player building this prefab:", MessageType.None);
            // TODO: Add building prefab selection dropdown
            EditorGUILayout.LabelField("Player Building Prefab", conditional.playerBuildingTrigger != null ? conditional.playerBuildingTrigger.PrefabName : "None (select building prefab)");

            // Response delay
            EditorGUILayout.LabelField("Response", EditorStyles.miniBoldLabel);
            conditional.delayAfterTrigger = EditorGUILayout.Slider("Wait Time (seconds)", conditional.delayAfterTrigger, 0f, 30f);

            // Slot actions
            EditorGUILayout.LabelField("Slot Actions", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Slots are specific positions on the enemy cruiser. The hull prefab determines which slot numbers correspond to which positions.\n\nSpecify exact slot number to target (destroys whatever is there, then replaces if specified).", MessageType.Info);

            if (conditional.slotActions == null) conditional.slotActions = new List<SlotReplacementAction>();

            for (int j = 0; j < conditional.slotActions.Count; j++)
            {
                SlotReplacementAction slotAction = conditional.slotActions[j];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField($"Slot Action {j + 1}", EditorStyles.miniBoldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Slot ID:", GUILayout.Width(60));
                slotAction.slotID = (byte)EditorGUILayout.IntField(slotAction.slotID, GUILayout.Width(40));
                var hullKey = currentConfig.cruiserPhases.Count > 0 ? currentConfig.cruiserPhases[0].hullKey : null;
                EditorGUILayout.LabelField($"({GetSlotTypeDescription(slotAction.slotID, hullKey)})", EditorStyles.miniLabel);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("Replacement Building", slotAction.replacementPrefab != null ? slotAction.replacementPrefab.PrefabName : "None (just destroy)");
                // TODO: Add building prefab selection dropdown
                slotAction.ignoreDroneReq = EditorGUILayout.Toggle("Ignore Drone Requirement", slotAction.ignoreDroneReq);
                slotAction.ignoreBuildTime = EditorGUILayout.Toggle("Ignore Build Time", slotAction.ignoreBuildTime);

                if (GUILayout.Button("Remove Slot Action", GUILayout.Width(150)))
                {
                    conditional.slotActions.RemoveAt(j);
                    EditorUtility.SetDirty(currentConfig);
                    break;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Slot Action", GUILayout.Width(150)))
            {
                conditional.slotActions.Add(new SlotReplacementAction());
                EditorUtility.SetDirty(currentConfig);
            }

            // Dialog
            EditorGUILayout.LabelField("Dialog", EditorStyles.miniBoldLabel);
            DrawChatKeySelector(ref conditional.chatKey);
            EditorGUILayout.HelpBox("Chat shown after all slot actions complete. Leave empty for no chat.", MessageType.None);

            if (GUILayout.Button("Remove Conditional Action"))
            {
                currentConfig.conditionalActions.RemoveAt(i);
                EditorUtility.SetDirty(currentConfig);
                break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Conditional Action"))
        {
            currentConfig.conditionalActions.Add(new ConditionalAction());
            EditorUtility.SetDirty(currentConfig);
        }
    }

    void DrawTestingTab()
    {
        EditorGUILayout.LabelField("Testing & Validation", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This tab will contain tools for validating ChainBattle configurations.", MessageType.Info);

        if (GUILayout.Button("Validate Configuration"))
        {
            ValidateConfiguration();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Translation Tools", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Generate prompts for AI translation agents to populate localization strings.", MessageType.Info);

        if (GUILayout.Button("Generate Translation Prompt"))
        {
            GenerateTranslationPrompt();
        }
    }

        private void ValidateConfiguration()
        {
            if (currentConfig == null)
            {
                EditorUtility.DisplayDialog("Validation Error", "No configuration to validate.", "OK");
                return;
            }

            var errors = new List<string>();
            var warnings = new List<string>();

            // Check basic requirements
            if (string.IsNullOrEmpty(currentConfig.levelNameKey))
                errors.Add("• Level name key is required (should match captain name).");

            if (currentConfig.levelNumber <= 0)
                errors.Add("• Level number must be greater than 0.");

            // Check battle settings
            if (currentConfig.musicKeys == null)
                errors.Add("• Background music is required.");
            
            if (string.IsNullOrEmpty(currentConfig.skyMaterialName))
                errors.Add("• Sky material is required.");
            
            if (currentConfig.captainExoKey == null)
                errors.Add("• Captain exoskeleton is required (applies to all phases).");

            // Check phases
            if (currentConfig.cruiserPhases == null || currentConfig.cruiserPhases.Count == 0)
            {
                errors.Add("• At least one cruiser phase is required.");
            }
            else
            {
                int finalPhaseCount = currentConfig.cruiserPhases.Count(phase => phase.isFinalPhase);
                if (finalPhaseCount == 0)
                    errors.Add("• At least one phase must be marked as final.");
                else if (finalPhaseCount > 1)
                    errors.Add($"• Only one phase can be marked as final (found {finalPhaseCount} final phases).");

                for (int i = 0; i < currentConfig.cruiserPhases.Count; i++)
                {
                    var phase = currentConfig.cruiserPhases[i];
                    int phaseNum = i + 1;

                    if (phase.hullKey == null)
                        errors.Add($"• Phase {phaseNum}: Missing hull key.");

                    if (phase.entryAnimationDuration < 0)
                        warnings.Add($"• Phase {phaseNum}: Entry animation duration is negative (will use 0).");

                    if (phase.phaseStartChats != null && phase.phaseStartChats.Count > 0)
                    {
                        for (int j = 0; j < phase.phaseStartChats.Count; j++)
                        {
                            var chat = phase.phaseStartChats[j];
                            if (string.IsNullOrEmpty(chat.chatKey))
                                warnings.Add($"• Phase {phaseNum}, Chat {j + 1}: Missing chat key.");
                        }
                    }
                }
            }

            // Check conditional actions
            if (currentConfig.conditionalActions != null)
            {
                for (int i = 0; i < currentConfig.conditionalActions.Count; i++)
                {
                    var action = currentConfig.conditionalActions[i];
                    if (action.playerBuildingTrigger == null)
                        warnings.Add($"• Conditional Action {i + 1}: No player building trigger specified.");
                    
                    if (action.slotActions != null && action.slotActions.Count == 0 && string.IsNullOrEmpty(action.chatKey))
                        warnings.Add($"• Conditional Action {i + 1}: No slot actions or chat specified (will do nothing).");
                }
            }

            // Show detailed results
            if (errors.Count > 0 || warnings.Count > 0)
            {
                string title = errors.Count > 0 ? $"Validation Failed ({errors.Count} error(s), {warnings.Count} warning(s))" : $"Validation Passed with Warnings ({warnings.Count} warning(s))";
                string message = "";
                
                if (errors.Count > 0)
                {
                    message += "ERRORS:\n";
                    message += string.Join("\n", errors) + "\n\n";
                }
                
                if (warnings.Count > 0)
                {
                    message += "WARNINGS:\n";
                    message += string.Join("\n", warnings);
                }

                EditorUtility.DisplayDialog(title, message, "OK");
                Debug.Log($"ChainBattle Validation Results:\n{message}");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Results", "✓ Configuration is valid! All required fields are set correctly.", "OK");
            }
        }

    void SaveChainBattleAsset()
    {
        if (currentConfig == null)
        {
            EditorUtility.DisplayDialog("Error", "No ChainBattle configuration to save!", "OK");
            return;
        }

        if (string.IsNullOrEmpty(currentConfig.levelNameKey))
        {
            EditorUtility.DisplayDialog("Error", "Level name key cannot be empty!", "OK");
            return;
        }

        if (currentConfig.levelNumber <= 0)
        {
            EditorUtility.DisplayDialog("Error", "Level number must be greater than 0!", "OK");
            return;
        }

        // Create Resources/ChainBattles directory if it doesn't exist
        string dirPath = "Assets/Resources/ChainBattles";
        if (!AssetDatabase.IsValidFolder(dirPath))
        {
            string[] folders = dirPath.Split('/');
            string currentPath = folders[0];
            for (int i = 1; i < folders.Length; i++)
            {
                if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                }
                currentPath += "/" + folders[i];
            }
        }

        // Save the asset
        string assetPath = $"{dirPath}/ChainBattle_{currentConfig.levelNumber}_{currentConfig.levelNameKey.Replace(" ", "").Replace("/", "_")}.asset";
        AssetDatabase.CreateAsset(currentConfig, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"ChainBattle saved as: {assetPath}", "OK");
    }

    // Helper methods for prefab selection UI
    private string[] GetHullOptions()
    {
        var hullKeys = StaticData.HullKeys;
        var options = new List<string> { "None" };
        options.AddRange(hullKeys.Select(hull => hull.PrefabName));
        return options.ToArray();
    }

    private string[] GetCaptainOptions()
    {
        var captainKeys = StaticPrefabKeys.CaptainExos.AllKeys;
        var options = new List<string> { "None" };
        options.AddRange(captainKeys.Select(captain => captain.PrefabName));
        return options.ToArray();
    }

    private int GetHullIndex(HullKey selectedHull)
    {
        if (selectedHull == null) return 0;
        var hullKeys = StaticData.HullKeys;
        var index = hullKeys.IndexOf(selectedHull);
        return index >= 0 ? index + 1 : 0; // +1 because "None" is at index 0
    }

    private int GetCaptainIndex(CaptainExoKey selectedCaptain)
    {
        if (selectedCaptain == null) return 0;
        var captainKeys = StaticPrefabKeys.CaptainExos.AllKeys;
        var index = captainKeys.ToList().FindIndex(captain => captain.PrefabName == selectedCaptain.PrefabName);
        return index >= 0 ? index + 1 : 0; // +1 because "None" is at index 0
    }

    private HullKey GetHullFromIndex(int index)
    {
        if (index == 0) return null; // "None" selected
        var hullKeys = StaticData.HullKeys;
        return index - 1 < hullKeys.Count ? hullKeys[index - 1] : null;
    }

    private CaptainExoKey GetCaptainFromIndex(int index)
    {
        if (index == 0) return null; // "None" selected
        var captainKeys = StaticPrefabKeys.CaptainExos.AllKeys;
        var captainList = captainKeys.ToList();
        return index - 1 < captainList.Count ? (CaptainExoKey)captainList[index - 1] : null;
    }

    // Helper methods for music selection
    private string[] GetMusicOptions()
    {
        return new string[] { "None", "Bobby", "Confusion", "Experimental", "Againagain", "Juggernaut", "Nothing", "Sleeper", "Fortress" };
    }

    private int GetMusicIndex(SoundKeyPair selectedMusic)
    {
        if (selectedMusic == null) return 0;
        // Compare by checking if the primary key matches known music tracks
        var options = GetMusicOptions();
        for (int i = 1; i < options.Length; i++)
        {
            var music = GetMusicFromIndex(i);
            if (music != null && music.PrimaryKey.Name == selectedMusic.PrimaryKey.Name)
                return i;
        }
        return 0;
    }

    private SoundKeyPair GetMusicFromIndex(int index)
    {
        if (index == 0) return null;
        var musicClass = typeof(BattleCruisers.Data.Static.SoundKeys.Music.Background);
        var options = GetMusicOptions();
        if (index >= options.Length) return null;
        
        string musicName = options[index];
        var property = musicClass.GetProperty(musicName);
        return property?.GetValue(null) as SoundKeyPair;
    }

    // Helper methods for sky selection
    private string[] GetSkyOptions()
    {
        var options = new List<string> { "None" };
        options.AddRange(SkyMaterials.All);
        return options.ToArray();
    }

    private int GetSkyIndex(string selectedSky)
    {
        if (string.IsNullOrEmpty(selectedSky)) return 0;
        var skyList = SkyMaterials.All.ToList();
        var index = skyList.IndexOf(selectedSky);
        return index >= 0 ? index + 1 : 0; // +1 because "None" is at index 0
    }

    private string GetSkyFromIndex(int index)
    {
        if (index == 0) return null;
        var skyList = SkyMaterials.All.ToList();
        return index - 1 < skyList.Count ? skyList[index - 1] : null;
    }

    // Helper methods for building selection
    private string[] GetBuildingOptions()
    {
        var buildingKeys = StaticData.BuildingKeys;
        var options = new List<string> { "None" };
        options.AddRange(buildingKeys.Select(building => building.PrefabName));
        return options.ToArray();
    }

    private int GetBuildingIndex(PrefabKeyName prefabKeyName)
    {
        if (prefabKeyName == 0) return 0; // None
        var buildingKeys = StaticData.BuildingKeys;
        try
        {
            var buildingKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(prefabKeyName);
            var index = buildingKeys.IndexOf(buildingKey);
            return index >= 0 ? index + 1 : 0; // +1 because "None" is at index 0
        }
        catch
        {
            return 0;
        }
    }

    private PrefabKeyName GetBuildingFromIndex(int index)
    {
        if (index == 0) return 0; // None
        var buildingKeys = StaticData.BuildingKeys;
        if (index - 1 >= buildingKeys.Count) return 0;
        
        var buildingKey = buildingKeys[index - 1];
        // Convert BuildingKey to PrefabKeyName by trying all enum values
        string prefabName = buildingKey.PrefabName;
        
        // Try direct match first
        string enumName = $"Building_{prefabName}";
        if (System.Enum.TryParse<PrefabKeyName>(enumName, true, out var result))
        {
            return result;
        }
        
        // Fallback: iterate through all Building_ prefixed enum values
        foreach (PrefabKeyName prefabKeyName in System.Enum.GetValues(typeof(PrefabKeyName)))
        {
            if (prefabKeyName.ToString().StartsWith("Building_"))
            {
                try
                {
                    var testKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(prefabKeyName);
                    if (testKey.PrefabName == prefabName)
                    {
                        return prefabKeyName;
                    }
                }
                catch
                {
                    // Continue searching
                }
            }
        }
        
        return 0; // Not found
    }

    // Helper methods for unit selection
    private string[] GetUnitOptions()
    {
        var unitKeys = StaticData.UnitKeys;
        var options = new List<string> { "None" };
        options.AddRange(unitKeys.Select(unit => unit.PrefabName));
        return options.ToArray();
    }

    private int GetUnitIndex(PrefabKeyName prefabKeyName)
    {
        if (prefabKeyName == 0) return 0; // None
        var unitKeys = StaticData.UnitKeys;
        try
        {
            var unitKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(prefabKeyName);
            var index = unitKeys.IndexOf(unitKey);
            return index >= 0 ? index + 1 : 0; // +1 because "None" is at index 0
        }
        catch
        {
            return 0;
        }
    }

    private PrefabKeyName GetUnitFromIndex(int index)
    {
        if (index == 0) return 0; // None
        var unitKeys = StaticData.UnitKeys;
        if (index - 1 >= unitKeys.Count) return 0;
        
        var unitKey = unitKeys[index - 1];
        // Convert UnitKey to PrefabKeyName by trying all enum values
        string prefabName = unitKey.PrefabName;
        
        // Try direct match first
        string enumName = $"Unit_{prefabName}";
        if (System.Enum.TryParse<PrefabKeyName>(enumName, true, out var result))
        {
            return result;
        }
        
        // Fallback: iterate through all Unit_ prefixed enum values
        foreach (PrefabKeyName prefabKeyName in System.Enum.GetValues(typeof(PrefabKeyName)))
        {
            if (prefabKeyName.ToString().StartsWith("Unit_"))
            {
                try
                {
                    var testKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(prefabKeyName);
                    if (testKey.PrefabName == prefabName)
                    {
                        return prefabKeyName;
                    }
                }
                catch
                {
                    // Continue searching
                }
            }
        }
        
        return 0; // Not found
    }

    private int GetNextAvailableLevelNumber()
    {
        var existingChainBattles = StaticData.ChainBattles;
        if (existingChainBattles == null || existingChainBattles.Count == 0)
        {
            return 1;
        }

        int maxLevel = existingChainBattles.Max(cb => cb.levelNumber);
        return maxLevel + 1;
    }

    private List<BodykitData> GetCompatibleBodykits(HullKey selectedHull)
    {
        if (selectedHull == null) return new List<BodykitData>();

        try
        {
            // Get the hull type for the selected hull
            var hullType = StaticPrefabKeys.Hulls.GetHullType(selectedHull);

            // Filter bodykits that are compatible with this hull type
            var compatibleBodykits = new List<BodykitData>();

            foreach (var bodykitData in StaticData.Bodykits)
            {
                try
                {
                    // Get the bodykit prefab path and load it directly
                    var bodykitKey = StaticPrefabKeys.BodyKits.GetBodykitKey(bodykitData.Index);
                    if (bodykitKey != null)
                    {
                        // Try loading the prefab directly from the correct addressables path
                        var prefabPath = "Assets/Resources_moved/" + bodykitKey.PrefabPath + ".prefab";
                        var bodykitPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<BattleCruisers.UI.ScreensScene.ProfileScreen.Bodykit>(prefabPath);

                        if (bodykitPrefab != null)
                        {
                            Debug.Log($"Bodykit {bodykitData.Index} ({bodykitKey.PrefabName}): CruiserType = {bodykitPrefab.CruiserType}, Target HullType = {hullType}");
                            if (bodykitPrefab.CruiserType == hullType)
                            {
                                compatibleBodykits.Add(bodykitData);
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Failed to load bodykit prefab at path: {prefabPath}");
                        }
                    }
                }
                catch (System.Exception e)
                {
                    // Skip problematic bodykits
                    Debug.LogWarning($"Skipping bodykit {bodykitData.Index} due to error: {e.Message}");
                }
            }

            return compatibleBodykits;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error getting compatible bodykits: {e.Message}");
            return new List<BodykitData>();
        }
    }

    private void CreateFeiDemoChainBattle()
    {
        // Create the ChainBattle configuration
        currentConfig = ScriptableObject.CreateInstance<ChainBattleConfiguration>();
        currentConfig.levelNumber = 999;
        currentConfig.levelNameKey = "CHAINBATTLE_FEI_NAME";

        // Set music, sky, and captain (Fei)
        currentConfig.musicKeys = SoundKeys.Music.Background.Fortress; // Boss music
        currentConfig.skyMaterialName = SkyMaterials.Purple; // BG with other cruisers closing in
        currentConfig.captainExoKey = StaticPrefabKeys.CaptainExos.GetCaptainExoKey(1); // Fei (captain 1)

        // Create Phase 1: Stealth Raptor - Fei introduction
        CruiserPhase phase1 = new CruiserPhase
        {
            hullKey = StaticPrefabKeys.Hulls.Raptor,
            bodykits = new BodykitData[0], // Will be selected via filtered UI
            isFinalPhase = false,
            entryAnimationDuration = 10,
            phaseStartChats = new List<ChainBattleChat>
            {
                new ChainBattleChat { chatKey = "level999/EnemyPhase1Start", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 4 },
                new ChainBattleChat { chatKey = "level999/PlayerPhase1Response", speaker = ChainBattleChat.SpeakerType.PlayerCaptain, displayDuration = 3 }
            },
            initialBuildings = new SequencePoint.BuildingAction[]
            {
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_ShieldGenerator, SlotID = 2, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_AntiShipTurret, SlotID = 4, IgnoreDroneReq = true, IgnoreBuildTime = true }
            },
            initialUnits = new SequencePoint.UnitAction[]
            {
                new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_Frigate, Postion = new Vector2(25, 0), SpawnArea = new Vector2(2, 2), Amount = 2, RequiredFactory = PrefabKeyName.Building_NavalFactory },
                new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_AttackBoat, Postion = new Vector2(28, 2), SpawnArea = new Vector2(1, 1), Amount = 3, RequiredFactory = PrefabKeyName.Building_NavalFactory }
            },
            phaseBonuses = new Cruiser.BoostStats[]
            {
                new Cruiser.BoostStats { boostType = BoostType.HealthAllBuilding, boostAmount = 3.0f }, // Building HP x3
                new Cruiser.BoostStats { boostType = BoostType.BuildRateAllBuilding, boostAmount = 0.5f } // +50% build speed
            },
            phaseSequencePoints = new SequencePoint[]
            {
                new SequencePoint
                {
                    DelayMS = 5000,
                    Faction = Faction.Reds,
                    BuildingActions = new List<SequencePoint.BuildingAction>
                    {
                        new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_Coastguard, SlotID = 0, IgnoreDroneReq = true, IgnoreBuildTime = true }
                    },
                    BoostActions = new List<SequencePoint.BoostAction>(),
                    UnitActions = new List<SequencePoint.UnitAction>
                    {
                        new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_GlassCannoneer, Postion = new Vector2(30, 5), SpawnArea = new Vector2(2, 2), Amount = 2 }
                    }
                }
            }
        };

        // Create Phase 2: Hammerhead - "That's just my point ship!"
        CruiserPhase phase2 = new CruiserPhase
        {
            hullKey = StaticPrefabKeys.Hulls.Hammerhead,
            bodykits = new BodykitData[0], // Will be selected via filtered UI
            isFinalPhase = false,
            entryAnimationDuration = 8,
            phaseStartChats = new List<ChainBattleChat>
            {
                new ChainBattleChat { chatKey = "level999/EnemyPhase2Start", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 4 },
                new ChainBattleChat { chatKey = "level999/PlayerPhase2Response", speaker = ChainBattleChat.SpeakerType.PlayerCaptain, displayDuration = 3 }
            },
            initialBuildings = new SequencePoint.BuildingAction[]
            {
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_AntiAirTurret, SlotID = 1, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_ShieldGenerator, SlotID = 2, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_GrapheneBarrier, SlotID = 3, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation, SlotID = 4, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation4, SlotID = 6, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation8, SlotID = 7, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_SamSite, SlotID = 5, IgnoreDroneReq = true, IgnoreBuildTime = true }
            },
            initialUnits = new SequencePoint.UnitAction[]
            {
                new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_Bomber, Postion = new Vector2(32, 8), SpawnArea = new Vector2(3, 3), Amount = 4, RequiredFactory = PrefabKeyName.Building_AirFactory },
                new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_Fighter, Postion = new Vector2(35, 3), SpawnArea = new Vector2(2, 2), Amount = 3, RequiredFactory = PrefabKeyName.Building_AirFactory }
            },
            phaseBonuses = new Cruiser.BoostStats[]
            {
                new Cruiser.BoostStats { boostType = BoostType.HealthAllBuilding, boostAmount = 2.0f }, // x2 building HP
                new Cruiser.BoostStats { boostType = BoostType.BuildRateOffense, boostAmount = 0.5f } // +50% offense build rate
            },
            phaseSequencePoints = new SequencePoint[]
            {
                new SequencePoint
                {
                    DelayMS = 3000,
                    Faction = Faction.Reds,
                    BuildingActions = new List<SequencePoint.BuildingAction>
                    {
                        new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_RocketLauncher, SlotID = 0, IgnoreDroneReq = true, IgnoreBuildTime = true }
                    },
                    BoostActions = new List<SequencePoint.BoostAction>(),
                    UnitActions = new List<SequencePoint.UnitAction>
                    {
                        new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_StratBomber, Postion = new Vector2(38, 10), SpawnArea = new Vector2(1, 1), Amount = 1 }
                    }
                }
            }
        };

        // Create Phase 3: Megalodon - "That's just my OTHER point ship!" (FINAL)
        CruiserPhase phase3 = new CruiserPhase
        {
            hullKey = StaticPrefabKeys.Hulls.Megalodon,
            bodykits = new BodykitData[0], // Will be selected via filtered UI
            isFinalPhase = true,
            entryAnimationDuration = 6,
            phaseStartChats = new List<ChainBattleChat>
            {
                new ChainBattleChat { chatKey = "level999/EnemyFinalPhase", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 5 },
                new ChainBattleChat { chatKey = "level999/EnemyDefeat", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 6 }
            },
            initialBuildings = new SequencePoint.BuildingAction[]
            {
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_Broadsides, SlotID = 1, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation6, SlotID = 2, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation8, SlotID = 3, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation8, SlotID = 4, IgnoreDroneReq = true, IgnoreBuildTime = true },
                new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_CIWS, SlotID = 6, IgnoreDroneReq = true, IgnoreBuildTime = true }
            },
            initialUnits = new SequencePoint.UnitAction[]
            {
                new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_Gunship, Postion = new Vector2(40, 12), SpawnArea = new Vector2(4, 4), Amount = 6, RequiredFactory = PrefabKeyName.Building_AirFactory },
                new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_Destroyer, Postion = new Vector2(35, 8), SpawnArea = new Vector2(3, 3), Amount = 4, RequiredFactory = PrefabKeyName.Building_NavalFactory }
            },
            phaseBonuses = new Cruiser.BoostStats[]
            {
                new Cruiser.BoostStats { boostType = BoostType.FireRateOffense, boostAmount = 2.0f }, // Enhanced offense
                new Cruiser.BoostStats { boostType = BoostType.BuildRateAllBuilding, boostAmount = 1.0f }, // Double build rate
                new Cruiser.BoostStats { boostType = BoostType.AccuracyAllBuilding, boostAmount = 2.0f } // +100% turret accuracy
            },
            phaseSequencePoints = new SequencePoint[]
            {
                new SequencePoint
                {
                    DelayMS = 2000,
                    Faction = Faction.Reds,
                    BuildingActions = new List<SequencePoint.BuildingAction>
                    {
                        new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DeathstarLauncher, SlotID = 0, IgnoreDroneReq = true, IgnoreBuildTime = true },
                        new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_FlakTurret, SlotID = 5, IgnoreDroneReq = true, IgnoreBuildTime = true },
                        new SequencePoint.BuildingAction { Operation = SequencePoint.BuildingAction.BuildingOp.Add, PrefabKeyName = PrefabKeyName.Building_DroneStation8, SlotID = 7, IgnoreDroneReq = true, IgnoreBuildTime = true }
                    },
                    BoostActions = new List<SequencePoint.BoostAction>(),
                    UnitActions = new List<SequencePoint.UnitAction>
                    {
                        new SequencePoint.UnitAction { PrefabKeyName = PrefabKeyName.Unit_ArchonBattleship, Postion = new Vector2(45, 15), SpawnArea = new Vector2(1, 1), Amount = 1 }
                    }
                }
            }
        };

        // Set phases
        currentConfig.cruiserPhases = new List<CruiserPhase> { phase1, phase2, phase3 };

        // Create conditional actions - Air Factory counter
        currentConfig.conditionalActions = new List<ConditionalAction>
        {
            new ConditionalAction
            {
                playerBuildingTrigger = StaticPrefabKeys.Buildings.AirFactory,
                delayAfterTrigger = 2f,
                slotActions = new List<SlotReplacementAction>
                {
                    new SlotReplacementAction { slotID = 3, replacementPrefab = StaticPrefabKeys.Buildings.FlakTurret, ignoreDroneReq = true, ignoreBuildTime = true },
                    new SlotReplacementAction { slotID = 7, replacementPrefab = StaticPrefabKeys.Buildings.AntiAirTurret, ignoreDroneReq = true, ignoreBuildTime = true }
                },
                chatKey = "level999/EnemyReactionAirFactory"
            }
        };

        // Create custom chats with cleaner naming and sample English text
        currentConfig.customChats = new List<ChainBattleChat>
        {
            new ChainBattleChat { chatKey = "level999/EnemyIntro", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 4, englishText = "You dare challenge me? This will be your end!" },
            new ChainBattleChat { chatKey = "level999/PlayerResponse", speaker = ChainBattleChat.SpeakerType.PlayerCaptain, displayDuration = 3, englishText = "I've faced worse than you. Bring it on!" },
            new ChainBattleChat { chatKey = "level999/EnemyTaunt", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 4, englishText = "Your ships are nothing compared to my fleet!" },
            new ChainBattleChat { chatKey = "level999/PlayerSurprise", speaker = ChainBattleChat.SpeakerType.PlayerCaptain, displayDuration = 3, englishText = "Wait, how did you know about my air factory?" },
            new ChainBattleChat { chatKey = "level999/EnemyFinalWarning", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 5, englishText = "This is your final warning. Surrender now!" },
            new ChainBattleChat { chatKey = "level999/EnemyCritical", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 6, englishText = "No! This can't be happening!" },
            new ChainBattleChat { chatKey = "level999/EnemyReactionAirFactory", speaker = ChainBattleChat.SpeakerType.EnemyCaptain, displayDuration = 4, englishText = "An air factory? Clever, but not clever enough!" },
            new ChainBattleChat { chatKey = "level999/DroneAppraisal", speaker = ChainBattleChat.SpeakerType.Narrative, displayDuration = 8, englishText = "Excellent work, Captain! The ChainBattle victory has unlocked new technologies." }
        };

        // Set the asset name and focus on it
        currentConfig.name = "ChainBattle_DemoLevel999";
        Selection.activeObject = currentConfig;

        Debug.Log("Created Demo ChainBattle configuration in editor");
    }

    private void DrawChatKeySelector(ref string chatKey)
    {
        var levelPrefix = $"level{currentConfig.levelNumber}/";

        // Get existing chat keys for this level (without prefix for cleaner display)
        var existingKeys = GetExistingChatKeysForLevel(currentConfig.levelNumber);
        var suggestedKeys = GetSuggestedChatKeys(currentConfig.levelNumber);

        // Convert to display names (remove prefix) and full keys
        var displayOptions = new List<string>();
        var fullKeyOptions = new List<string>();

        // Add existing keys first
        foreach (var key in existingKeys)
        {
            if (key.StartsWith(levelPrefix))
            {
                displayOptions.Add(key.Substring(levelPrefix.Length));
                fullKeyOptions.Add(key);
            }
        }

        // Add suggested keys (avoid duplicates)
        foreach (var key in suggestedKeys)
        {
            if (!fullKeyOptions.Contains(key))
            {
                displayOptions.Add(key.Substring(levelPrefix.Length));
                fullKeyOptions.Add(key);
            }
        }

        // Add current key if not already in list
        if (!string.IsNullOrEmpty(chatKey))
        {
            if (chatKey.StartsWith(levelPrefix))
            {
                var displayName = chatKey.Substring(levelPrefix.Length);
                if (!displayOptions.Contains(displayName))
                {
                    displayOptions.Insert(0, displayName);
                    fullKeyOptions.Insert(0, chatKey);
                }
            }
            else
            {
                // Handle keys that don't follow the pattern
                displayOptions.Insert(0, chatKey);
                fullKeyOptions.Insert(0, chatKey);
            }
        }

        // Show the selector
        if (displayOptions.Count > 0)
        {
            int selectedIndex = 0;
            if (!string.IsNullOrEmpty(chatKey))
            {
                if (chatKey.StartsWith(levelPrefix))
                {
                    var displayName = chatKey.Substring(levelPrefix.Length);
                    selectedIndex = displayOptions.IndexOf(displayName);
                }
                else
                {
                    selectedIndex = displayOptions.IndexOf(chatKey);
                }
                if (selectedIndex == -1) selectedIndex = 0;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Chat:", GUILayout.Width(40));
            int newIndex = EditorGUILayout.Popup(selectedIndex, displayOptions.ToArray());

            if (GUILayout.Button("✏", GUILayout.Width(25)))
            {
                // Allow manual editing of the full key
                chatKey = EditorGUILayout.TextField(chatKey, GUILayout.MinWidth(150));
            }
            else
            {
                chatKey = fullKeyOptions[newIndex];
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            chatKey = EditorGUILayout.TextField("Chat Key", chatKey);
        }

        // Show help text only if key doesn't follow expected pattern
        if (string.IsNullOrEmpty(chatKey) || !chatKey.StartsWith(levelPrefix))
        {
            EditorGUILayout.HelpBox($"Expected format: {levelPrefix}ChatName (dropdown shows available options)", MessageType.Info);
        }
    }

    private Dictionary<string, int> GetCruiserSlotCounts(IPrefabKey hullKey)
    {
        var slotCounts = new Dictionary<string, int>();

        if (hullKey == null) return slotCounts;

        try
        {
            // Get the cruiser prefab
            var cruiserPrefab = PrefabFactory.GetCruiserPrefab(hullKey as HullKey);
            if (cruiserPrefab == null) return slotCounts;

            // Get the cruiser component to access slot information
            var cruiser = cruiserPrefab.GetComponent<Cruiser>();
            if (cruiser == null || cruiser.SlotWrapperController == null) return slotCounts;

            // Count slots by type
            foreach (var slot in cruiser.SlotWrapperController.Slots)
            {
                var slotTypeName = GetSlotTypeName(slot.Type);
                if (!slotCounts.ContainsKey(slotTypeName))
                    slotCounts[slotTypeName] = 0;
                slotCounts[slotTypeName]++;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error getting slot counts for hull {hullKey}: {e.Message}");
        }

        return slotCounts;
    }

    private void DisplayHullSlotInfo(HullKey hullKey)
    {
        if (hullKey == null) return;

        var slotCounts = GetCruiserSlotCounts(hullKey);

        if (slotCounts.Count > 0)
        {
            var slotInfo = new System.Text.StringBuilder("Available slots: ");
            foreach (var kvp in slotCounts.OrderBy(kvp => kvp.Key))
            {
                slotInfo.Append($"{kvp.Value} {kvp.Key}, ");
            }
            // Remove trailing comma and space
            if (slotInfo.Length > 2)
                slotInfo.Length -= 2;

            EditorGUILayout.HelpBox(slotInfo.ToString(), MessageType.Info);
        }
    }

    private void ShowBuildingSelectionForSlot(byte slotId, string slotType, CruiserPhase phase)
    {
        // Get compatible buildings for this slot type
        var compatibleBuildings = GetCompatibleBuildingsForSlotType(slotType);

        // Show selection dialog
        var menu = new GenericMenu();

        // Add "None" option
        menu.AddItem(new GUIContent("None"), false, () => {
            RemoveBuildingFromSlot(slotId, phase);
        });

        menu.AddSeparator("");

        // Add compatible buildings
        foreach (var building in compatibleBuildings)
        {
            var buildingName = GetBuildingDisplayName(building);
            menu.AddItem(new GUIContent(buildingName), false, () => {
                AddBuildingToSlot(slotId, building, phase);
            });
        }

        menu.ShowAsContext();
    }

    private void AddBuildingToSlot(byte slotId, PrefabKeyName buildingKeyName, CruiserPhase phase)
    {
        // Remove any existing building in this slot
        RemoveBuildingFromSlot(slotId, phase);

        // Add new building action
        var buildingAction = new SequencePoint.BuildingAction
        {
            Operation = SequencePoint.BuildingAction.BuildingOp.Add,
            PrefabKeyName = buildingKeyName,
            SlotID = slotId,
            IgnoreDroneReq = true, // Default to ignoring drone requirements for AI
            IgnoreBuildTime = true  // Default to instant build for AI
        };

        var buildingList = new List<SequencePoint.BuildingAction>(phase.initialBuildings ?? new SequencePoint.BuildingAction[0]);
        buildingList.Add(buildingAction);
        phase.initialBuildings = buildingList.ToArray();

        EditorUtility.SetDirty(currentConfig);
    }

    private void RemoveBuildingFromSlot(byte slotId, CruiserPhase phase)
    {
        if (phase.initialBuildings == null) return;

        var buildingList = new List<SequencePoint.BuildingAction>(phase.initialBuildings);
        buildingList.RemoveAll(b => b.SlotID == slotId && b.Operation == SequencePoint.BuildingAction.BuildingOp.Add);
        phase.initialBuildings = buildingList.ToArray();

        EditorUtility.SetDirty(currentConfig);
    }

    private string GetBuildingDisplayName(PrefabKeyName buildingKeyName)
    {
        if (buildingKeyName == 0) return "None";

        try
        {
            var buildingKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
            return buildingKey?.PrefabName ?? buildingKeyName.ToString();
        }
        catch
        {
            return buildingKeyName.ToString();
        }
    }

    private List<PrefabKeyName> GetCompatibleBuildingsForSlotType(string slotType)
    {
        var compatibleBuildings = new List<PrefabKeyName>();

        // Map slot types to building categories
        switch (slotType.ToLower())
        {
            case "deck":
                // All buildings can go on deck slots (tactical, defense, offence)
                compatibleBuildings.AddRange(new[] {
                    PrefabKeyName.Building_ShieldGenerator,
                    PrefabKeyName.Building_StealthGenerator,
                    PrefabKeyName.Building_SpySatelliteLauncher,
                    PrefabKeyName.Building_LocalBooster,
                    PrefabKeyName.Building_ControlTower,
                    PrefabKeyName.Building_GrapheneBarrier,
                    PrefabKeyName.Building_AntiShipTurret,
                    PrefabKeyName.Building_AntiAirTurret,
                    PrefabKeyName.Building_Mortar,
                    PrefabKeyName.Building_SamSite,
                    PrefabKeyName.Building_TeslaCoil,
                    PrefabKeyName.Building_Coastguard,
                    PrefabKeyName.Building_FlakTurret,
                    PrefabKeyName.Building_CIWS,
                    PrefabKeyName.Building_Artillery,
                    PrefabKeyName.Building_RocketLauncher,
                    PrefabKeyName.Building_Railgun,
                    PrefabKeyName.Building_MLRS,
                    PrefabKeyName.Building_GatlingMortar,
                    PrefabKeyName.Building_MissilePod,
                    PrefabKeyName.Building_IonCannon,
                    PrefabKeyName.Building_Cannon,
                    PrefabKeyName.Building_BlastVLS,
                    PrefabKeyName.Building_FirecrackerVLS
                });
                break;

            case "platform":
                // Platform slots can have factories
                compatibleBuildings.AddRange(new[] {
                    PrefabKeyName.Building_AirFactory,
                    PrefabKeyName.Building_NavalFactory,
                    PrefabKeyName.Building_DroneFactory,
                    PrefabKeyName.Building_DroneStation,
                    PrefabKeyName.Building_DroneStation4,
                    PrefabKeyName.Building_DroneStation6,
                    PrefabKeyName.Building_DroneStation8
                });
                break;

            case "bow":
                // Bow slots typically have ultra weapons
                compatibleBuildings.AddRange(new[] {
                    PrefabKeyName.Building_DeathstarLauncher,
                    PrefabKeyName.Building_NukeLauncher,
                    PrefabKeyName.Building_Ultralisk,
                    PrefabKeyName.Building_KamikazeSignal,
                    PrefabKeyName.Building_Broadsides,
                    PrefabKeyName.Building_NovaArtillery,
                    PrefabKeyName.Building_UltraCIWS,
                    PrefabKeyName.Building_GlobeShield,
                    PrefabKeyName.Building_Sledgehammer,
                    PrefabKeyName.Building_RailCannon
                });
                break;

            case "mast":
                // Mast slots for tactical buildings
                compatibleBuildings.AddRange(new[] {
                    PrefabKeyName.Building_SpySatelliteLauncher,
                    PrefabKeyName.Building_LocalBooster,
                    PrefabKeyName.Building_ControlTower
                });
                break;

            case "utility":
                // Utility slots for various support buildings
                compatibleBuildings.AddRange(new[] {
                    PrefabKeyName.Building_DroneStation,
                    PrefabKeyName.Building_DroneStation4,
                    PrefabKeyName.Building_DroneStation6,
                    PrefabKeyName.Building_DroneStation8,
                    PrefabKeyName.Building_StealthGenerator,
                    PrefabKeyName.Building_GrapheneBarrier
                });
                break;
        }

        return compatibleBuildings;
    }

    private List<byte> GetCompatibleSlotsForBuilding(HullKey hullKey, BattleCruisers.Utils.PrefabKeyName buildingKeyName)
    {
        var compatibleSlots = new List<byte>();

        if (hullKey == null) return compatibleSlots;

        try
        {
            // Convert PrefabKeyName to IPrefabKey
            var buildingKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
            if (buildingKey == null) return compatibleSlots;

            var buildingWrapper = PrefabFactory.GetBuildingWrapperPrefab(buildingKey);
            if (buildingWrapper == null) return compatibleSlots;

            var requiredSlotType = buildingWrapper.Buildable.SlotSpecification.SlotType;

            // Get the cruiser to find compatible slots
            var cruiserPrefab = PrefabFactory.GetCruiserPrefab(hullKey);
            if (cruiserPrefab != null)
            {
                var cruiser = cruiserPrefab.GetComponent<Cruiser>();
                if (cruiser != null && cruiser.SlotWrapperController != null)
                {
                    var slots = cruiser.SlotWrapperController.Slots;
                    for (byte i = 0; i < slots.Count; i++)
                    {
                        if (slots[i].Type == requiredSlotType)
                        {
                            compatibleSlots.Add(i);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error getting compatible slots for building {buildingKeyName}: {e.Message}");
        }

        return compatibleSlots;
    }

    private string GetSlotTypeName(BattleCruisers.Cruisers.Slots.SlotType slotType)
    {
        switch (slotType)
        {
            case BattleCruisers.Cruisers.Slots.SlotType.Deck: return "Deck";
            case BattleCruisers.Cruisers.Slots.SlotType.Platform: return "Platform";
            case BattleCruisers.Cruisers.Slots.SlotType.Bow: return "Bow";
            case BattleCruisers.Cruisers.Slots.SlotType.Mast: return "Mast";
            case BattleCruisers.Cruisers.Slots.SlotType.Utility: return "Utility";
            default: return slotType.ToString();
        }
    }

    private string GetSlotTypeDescription(byte slotID, IPrefabKey hullKey = null)
    {
        if (hullKey == null) return $"Slot {slotID}";

        try
        {
            // Get the actual slot type from the hull prefab
            var cruiserPrefab = PrefabFactory.GetCruiserPrefab(hullKey as HullKey);
            if (cruiserPrefab != null)
            {
                var cruiser = cruiserPrefab.GetComponent<Cruiser>();
                if (cruiser != null && cruiser.SlotWrapperController != null)
                {
                    var slots = cruiser.SlotWrapperController.Slots;
                    if (slotID < slots.Count)
                    {
                        var slotType = slots[slotID].Type;
                        var typeName = GetSlotTypeName(slotType);
                        return $"{typeName} Slot {slotID}";
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error getting slot type for hull {hullKey}: {e.Message}");
        }

        return $"Slot {slotID}";
    }

    private void GenerateDefaultLocalizationKeys()
    {
        if (currentConfig == null) return;

        var levelPrefix = $"level{currentConfig.levelNumber}/";

        // Set the level name key if not set
        if (string.IsNullOrEmpty(currentConfig.levelNameKey))
        {
            currentConfig.levelNameKey = $"{levelPrefix}PlayerName";
        }

        // Add default chat keys if none exist
        if (currentConfig.customChats == null || currentConfig.customChats.Count == 0)
        {
            currentConfig.customChats = new List<ChainBattleChat>
            {
                new ChainBattleChat { chatKey = $"{levelPrefix}PlayerChat1", speaker = ChainBattleChat.SpeakerType.PlayerCaptain, displayDuration = 4 }
            };
        }

        EditorUtility.SetDirty(currentConfig);

        // Show info about what was created
        EditorUtility.DisplayDialog("Default Keys Generated",
            $"Created essential localization keys for level {currentConfig.levelNumber}:\n\n" +
            $"• {currentConfig.levelNameKey} (level display name)\n" +
            $"• {levelPrefix}PlayerText (player trash talk)\n" +
            $"• {levelPrefix}EnemyText (enemy trash talk)\n" +
            $"• {levelPrefix}DroneText (post-battle drone)\n" +
            $"• {levelPrefix}PlayerChat1 (starting chat)\n\n" +
            "Use the Translation Prompt button to generate AI instructions for populating these keys in all languages.",
            "OK");
    }

    private void GenerateDefaultStoryKeys()
    {
        if (currentConfig == null) return;

        var levelPrefix = $"level{currentConfig.levelNumber}/";

        if (currentConfig.customChats == null)
            currentConfig.customChats = new List<ChainBattleChat>();

        // Add story keys if they don't exist
        var storyKeys = new[]
        {
            ($"{levelPrefix}PlayerText", ChainBattleChat.SpeakerType.PlayerCaptain, "Trash talk from player before battle"),
            ($"{levelPrefix}EnemyText", ChainBattleChat.SpeakerType.EnemyCaptain, "Trash talk from enemy before battle"),
            ($"{levelPrefix}DroneText", ChainBattleChat.SpeakerType.Narrative, "Post-battle drone appraisal message")
        };

        foreach (var (key, speaker, description) in storyKeys)
        {
            if (!currentConfig.customChats.Any(c => c.chatKey == key))
            {
                currentConfig.customChats.Add(new ChainBattleChat
                {
                    chatKey = key,
                    speaker = speaker,
                    displayDuration = 4f,
                    englishText = description
                });
            }
        }

        EditorUtility.SetDirty(currentConfig);
    }

    private void AddNewChatKey(string chatType)
    {
        if (currentConfig == null) return;

        // Find the next available number for this chat type
        int nextNumber = 1;
        var levelPrefix = $"level{currentConfig.levelNumber}/";

        if (currentConfig.customChats != null)
        {
            foreach (var chat in currentConfig.customChats)
            {
                if (chat.chatKey != null && chat.chatKey.StartsWith($"{levelPrefix}{chatType}"))
                {
                    var suffix = chat.chatKey.Substring($"{levelPrefix}{chatType}".Length);
                    if (int.TryParse(suffix, out int num) && num >= nextNumber)
                    {
                        nextNumber = num + 1;
                    }
                }
            }
        }

        // Create the new chat key
        var newChatKey = $"{levelPrefix}{chatType}{nextNumber}";

        // Determine speaker type based on chat type
        var speaker = ChainBattleChat.SpeakerType.EnemyCaptain;
        if (chatType.Contains("Player"))
            speaker = ChainBattleChat.SpeakerType.PlayerCaptain;
        else if (chatType.Contains("Drone"))
            speaker = ChainBattleChat.SpeakerType.Narrative;

        // Add the new chat
        if (currentConfig.customChats == null)
            currentConfig.customChats = new List<ChainBattleChat>();

        currentConfig.customChats.Add(new ChainBattleChat
        {
            chatKey = newChatKey,
            speaker = speaker,
            displayDuration = 4f
        });

        EditorUtility.SetDirty(currentConfig);
    }

    private string GetEnglishTextForKey(string chatKey)
    {
        if (currentConfig == null) return null;

        // Check phase start chats
        if (currentConfig.cruiserPhases != null)
        {
            foreach (var phase in currentConfig.cruiserPhases)
            {
                if (phase.phaseStartChats != null)
                {
                    foreach (var chat in phase.phaseStartChats)
                    {
                        if (chat.chatKey == chatKey && !string.IsNullOrEmpty(chat.englishText))
                        {
                            return chat.englishText;
                        }
                    }
                }
            }
        }

        // Check custom chats
        if (currentConfig.customChats != null)
        {
            foreach (var chat in currentConfig.customChats)
            {
                if (chat.chatKey == chatKey && !string.IsNullOrEmpty(chat.englishText))
                {
                    return chat.englishText;
                }
            }
        }

        // Check reactive chats
        if (currentConfig.conditionalActions != null)
        {
            foreach (var action in currentConfig.conditionalActions)
            {
                if (action.chatKey == chatKey && !string.IsNullOrEmpty(action.chatKey))
                {
                    // For reactive actions, we don't have english text fields, so return a placeholder
                    return "[Reactive chat - context: " + action.chatKey + "]";
                }
            }
        }

        return null;
    }


    private List<string> GetExistingChatKeysForLevel(int levelNumber)
    {
        var keys = new List<string>();
        var levelPrefix = $"level{levelNumber}/";

        try
        {
            // Access the StoryTable to get all keys that start with our level prefix
            var storyTable = LocTableCache.StoryTable;
            if (storyTable != null && storyTable.Handle.IsValid())
            {
                var stringTable = storyTable.Handle.Result;
                if (stringTable != null)
                {
                    // Get all table entries and filter by our level prefix
                    foreach (var entry in stringTable)
                    {
                        var keyString = entry.Key.ToString();
                        if (keyString.StartsWith(levelPrefix))
                        {
                            keys.Add(keyString);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error fetching existing chat keys for level {levelNumber}: {e.Message}");
        }

        return keys.OrderBy(k => k).ToList();
    }

    private List<string> GetSuggestedChatKeys(int levelNumber)
    {
        var levelPrefix = $"level{levelNumber}/";
        return new List<string>
        {
            $"{levelPrefix}PlayerText",
            $"{levelPrefix}EnemyText",
            $"{levelPrefix}DroneText",
            $"{levelPrefix}PlayerName",
            $"{levelPrefix}PlayerResponse",
            $"{levelPrefix}PlayerSurprise",
            $"{levelPrefix}PlayerChallenge",
            $"{levelPrefix}EnemyIntro",
            $"{levelPrefix}EnemyTaunt",
            $"{levelPrefix}EnemyWarning",
            $"{levelPrefix}EnemyFinalPhase",
            $"{levelPrefix}EnemyDefeat",
            $"{levelPrefix}EnemyPhase1Start",
            $"{levelPrefix}EnemyPhase2Start",
            $"{levelPrefix}EnemyReactionAirFactory",
            $"{levelPrefix}EnemyReactionShield",
            $"{levelPrefix}EnemyReactionTurret",
            $"{levelPrefix}DroneAppraisal",
            $"{levelPrefix}DroneVictory",
            $"{levelPrefix}DroneDefeat"
        };
    }

    private void GenerateTranslationPrompt()
    {
        if (currentConfig == null)
        {
            EditorUtility.DisplayDialog("Error", "No ChainBattle configuration loaded.", "OK");
            return;
        }

        // Collect all unique chat keys from the configuration
        var chatKeys = new HashSet<string>();

        // Add level name key
        if (!string.IsNullOrEmpty(currentConfig.levelNameKey))
        {
            chatKeys.Add(currentConfig.levelNameKey);
        }

        // Add phase start chats
        if (currentConfig.cruiserPhases != null)
        {
            foreach (var phase in currentConfig.cruiserPhases)
            {
                if (phase.phaseStartChats != null)
                {
                    foreach (var chat in phase.phaseStartChats)
                    {
                        if (!string.IsNullOrEmpty(chat.chatKey))
                        {
                            chatKeys.Add(chat.chatKey);
                        }
                    }
                }
            }
        }

        // Add custom chats
        if (currentConfig.customChats != null)
        {
            foreach (var chat in currentConfig.customChats)
            {
                if (!string.IsNullOrEmpty(chat.chatKey))
                {
                    chatKeys.Add(chat.chatKey);
                }
            }
        }

        // Add reactive chats
        if (currentConfig.conditionalActions != null)
        {
            foreach (var action in currentConfig.conditionalActions)
            {
                if (!string.IsNullOrEmpty(action.chatKey))
                {
                    chatKeys.Add(action.chatKey);
                }
            }
        }

        // Generate the translation prompt
        var prompt = GenerateTranslationPromptText(chatKeys.ToList(), currentConfig.levelNumber);

        // Copy to clipboard
        GUIUtility.systemCopyBuffer = prompt;

        // Show confirmation
        EditorUtility.DisplayDialog("Translation Prompt Generated",
            "Translation prompt has been copied to clipboard! Paste it into your AI translator agent.",
            "OK");
    }

    private string GenerateTranslationPromptText(List<string> chatKeys, int levelNumber)
    {
        var sb = new StringBuilder();

        sb.AppendLine("TRANSLATION REQUEST: ChainBattle Level Localization");
        sb.AppendLine("================================================");
        sb.AppendLine();
        sb.AppendLine("You are an AI translator agent with full access to the BattleCruisers Unity project codebase. Your task is to translate ChainBattle localization strings for level " + levelNumber + ".");
        sb.AppendLine();
        sb.AppendLine("CONTEXT:");
        sb.AppendLine("- This is a ChainBattle (multi-phase boss fight) in the BattleCruisers game");
        sb.AppendLine("- The game uses Unity's Localization system with StringTable assets");
        sb.AppendLine("- English strings are in the StoryTable collection");
        sb.AppendLine("- All target languages should be populated in their respective StringTable entries");
        sb.AppendLine();
        sb.AppendLine("REQUIREMENTS:");
        sb.AppendLine("1. Translate ALL English strings from StoryTable/level" + levelNumber + "/ entries to all non-English languages");
        sb.AppendLine("2. Maintain consistent character names - if a character name appears in multiple entries, use the same translation");
        sb.AppendLine("3. Preserve game terminology and proper nouns from existing translations in the StoryTable");
        sb.AppendLine("4. Keep the same tone and style appropriate for a sci-fi strategy game");
        sb.AppendLine("5. Pay special attention to battle dialogue - it should sound natural and fitting for space combat");
        sb.AppendLine();
        sb.AppendLine("CHAINBATTLE SPECIFIC GUIDANCE:");
        sb.AppendLine("- *Intro/*Response: Initial phase dialogue between captains");
        sb.AppendLine("- *Taunt/*Surprise/*Challenge: Battle banter and reactions");
        sb.AppendLine("- *Warning/*FinalPhase/*Defeat: Phase transitions and defeat dialogue");
        sb.AppendLine("- *Phase#Start: Chat when entering specific phases");
        sb.AppendLine("- *Reaction*: Enemy reactions to player building actions");
        sb.AppendLine("- Drone*: Post-battle AI assistant commentary");
        sb.AppendLine("- PlayerText/EnemyText: Pre-battle trash talk screen");
        sb.AppendLine("- PlayerName: Display name for the level/chain battle");
        sb.AppendLine();
        sb.AppendLine("LOCALIZATION KEYS TO TRANSLATE:");
        sb.AppendLine("(All keys are in the StoryTable collection)");
        sb.AppendLine();

        // Include English text where available
        foreach (var key in chatKeys.OrderBy(k => k))
        {
            sb.Append("- " + key);

            // Try to find the English text for this key
            var englishText = GetEnglishTextForKey(key);
            if (!string.IsNullOrEmpty(englishText))
            {
                sb.Append(" → \"" + englishText.Replace("\"", "\\\"") + "\"");
            }

            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine("INSTRUCTIONS FOR IMPLEMENTATION:");
        sb.AppendLine("1. Access the Unity project at the specified path");
        sb.AppendLine("2. Open the StoryTable StringTable asset");
        sb.AppendLine("3. For each English entry under level" + levelNumber + "/, create/update translations in all language tables");
        sb.AppendLine("4. Use existing StoryTable translations as reference for character names and game terminology");
        sb.AppendLine("5. Save all StringTable assets after translation");
        sb.AppendLine();
        sb.AppendLine("VERIFICATION:");
        sb.AppendLine("- Ensure all level" + levelNumber + "/ keys have translations in all supported languages");
        sb.AppendLine("- Test that the ChainBattle displays correctly in each language");
        sb.AppendLine("- Confirm character name consistency across all dialogue");
        sb.AppendLine();
        sb.AppendLine("END OF TRANSLATION REQUEST");

        return sb.ToString();
    }

    // Additional tab implementations would follow similar patterns...
}
