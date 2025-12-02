using UnityEngine;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using System.Linq;
using BattleCruisers.Data.Static;
using UnityEditor;

public class PlayerSaveGameEditor : EditorWindow
{
    private GameModel gameModel;
    private List<bool> sideQuestCompletion;
    private int mainQuestsUnlocked;
    private Vector2 scrollPosition = Vector2.zero;

    // Editable fields
    private long editableCoins;
    private long editableCredits;
    private long editableLifetimeDestructionScore;
    private bool editableHasAttemptedTutorial;
    private int editableSelectedLevel;
    private int editableSelectedSideQuestID;
    private int editableNumOfLevelsCompleted;

    // Additional editable fields
    private string editablePlayerName;
    private float editableBattleWinScore;
    private int editableTimesLostOnLastLevel;
    private int editableBounty;
    private long editableBestDestructionScore;
    private bool editablePremiumEdition;
    private bool editableHasSyncdShop;
    private int editableGameMap;
    private string editableQueueName;
    private int editableID_Bodykit_AIbot;
    private int editableSelectedPvPLevel;

    [MenuItem("Tools/Player Save Game Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerSaveGameEditor>("Player Save Game Editor");
    }

    private void OnEnable()
    {
        LoadGameModel();
        sideQuestCompletion = new List<bool>(new bool[StaticData.SideQuests.Count]);
    }

    private void OnGUI()
    {
        if (gameModel == null)
        {
            EditorGUILayout.HelpBox("GameModel or ApplicationModel is not available.", MessageType.Warning);
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Latest Values"))
        {
            LoadGameModel();
        }

        if (GUILayout.Button("Save Changes"))
        {
            SaveGameModel();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Game State - EDITABLE", EditorStyles.boldLabel);

        // Editable fields
        editableCoins = EditorGUILayout.LongField("Coins:", editableCoins);
        editableCredits = EditorGUILayout.LongField("Credits:", editableCredits);
        editableLifetimeDestructionScore = EditorGUILayout.LongField("Lifetime Destruction Score:", editableLifetimeDestructionScore);
        editableHasAttemptedTutorial = EditorGUILayout.Toggle("Tutorial Completed:", editableHasAttemptedTutorial);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Player Profile - EDITABLE", EditorStyles.boldLabel);
        editablePlayerName = EditorGUILayout.TextField("Player Name:", editablePlayerName);
        editableBattleWinScore = EditorGUILayout.FloatField("Battle Win Score:", editableBattleWinScore);
        editableTimesLostOnLastLevel = EditorGUILayout.IntField("Times Lost on Last Level:", editableTimesLostOnLastLevel);
        editableBounty = EditorGUILayout.IntField("Bounty:", editableBounty);
        editableBestDestructionScore = EditorGUILayout.LongField("Best Destruction Score:", editableBestDestructionScore);
        editablePremiumEdition = EditorGUILayout.Toggle("Premium Edition:", editablePremiumEdition);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Game State - EDITABLE", EditorStyles.boldLabel);
        editableHasSyncdShop = EditorGUILayout.Toggle("Has Synced Shop:", editableHasSyncdShop);
        editableGameMap = EditorGUILayout.IntField("Game Map:", editableGameMap);
        editableQueueName = EditorGUILayout.TextField("Queue Name:", editableQueueName);
        editableID_Bodykit_AIbot = EditorGUILayout.IntField("ID Bodykit AIbot:", editableID_Bodykit_AIbot);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Application Model Details - EDITABLE", EditorStyles.boldLabel);
        editableSelectedLevel = EditorGUILayout.IntField("Selected Level:", editableSelectedLevel);
        editableSelectedPvPLevel = EditorGUILayout.IntField("Selected PvP Level:", editableSelectedPvPLevel);
        editableSelectedSideQuestID = EditorGUILayout.IntField("Selected Side Quest ID:", editableSelectedSideQuestID);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("READ-ONLY INFO", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Main Quests Unlocked: {mainQuestsUnlocked}");
        EditorGUILayout.LabelField($"Completed Side Quests: {sideQuestCompletion.Count(sq => sq)}");
        EditorGUILayout.LabelField($"Game Mode: {ApplicationModel.Mode}");
        EditorGUILayout.LabelField($"Num of Levels Completed: {gameModel.NumOfLevelsCompleted}");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Completed Side Quests:", EditorStyles.boldLabel);
        for (int i = 0; i < sideQuestCompletion.Count; i++)
        {
            bool newValue = EditorGUILayout.Toggle($"Side Quest {i}:", sideQuestCompletion[i]);
            if (newValue != sideQuestCompletion[i])
            {
                sideQuestCompletion[i] = newValue;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply Side Quest Changes"))
        {
            ApplySideQuestChanges();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("RESET SAVE DATA"))
        {
            if (EditorUtility.DisplayDialog("Reset Save Data",
                "This will DELETE all save data and create a fresh save! This cannot be undone. Are you sure?",
                "Yes, Reset Everything", "Cancel"))
            {
                ResetSaveData();
            }
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void LoadGameModel()
    {
        if (DataProvider.GameModel == null)
        {
            Debug.LogError("DataProvider or GameModel is not initialized.");
            return;
        }

        gameModel = DataProvider.GameModel;

        if (gameModel == null)
        {
            Debug.LogError("GameModel or ApplicationModel is not of type GameModel or ApplicationModel.");
            return;
        }

        // Load editable values
        editableCoins = gameModel.Coins;
        editableCredits = gameModel.Credits;
        editableLifetimeDestructionScore = gameModel.LifetimeDestructionScore;
        editableHasAttemptedTutorial = gameModel.HasAttemptedTutorial;
        editableSelectedLevel = ApplicationModel.SelectedLevel;
        editableSelectedSideQuestID = ApplicationModel.SelectedSideQuestID;

        // Load additional editable values
        editablePlayerName = gameModel.PlayerName;
        editableBattleWinScore = gameModel.BattleWinScore;
        editableTimesLostOnLastLevel = gameModel.TimesLostOnLastLevel;
        editableBounty = gameModel.Bounty;
        editableBestDestructionScore = gameModel.BestDestructionScore;
        editablePremiumEdition = gameModel.PremiumEdition;
        editableHasSyncdShop = gameModel.HasSyncdShop;
        editableGameMap = gameModel.GameMap;
        editableQueueName = gameModel.QueueName;
        editableID_Bodykit_AIbot = gameModel.ID_Bodykit_AIbot;
        editableSelectedPvPLevel = gameModel.SelectedPvPLevel;

        mainQuestsUnlocked = gameModel.CompletedLevels.Count;
        sideQuestCompletion = new List<bool>(new bool[StaticData.SideQuests.Count]);

        foreach (var completedQuest in gameModel.CompletedSideQuests)
        {
            int index = completedQuest.LevelNum;
            if (index >= 0 && index < sideQuestCompletion.Count)
            {
                sideQuestCompletion[index] = true;
            }
            else
            {
                Debug.LogWarning($"Completed side quest index {index} is out of range.");
            }
        }
    }

    private void SaveGameModel()
    {
        if (gameModel == null)
        {
            Debug.LogError("Cannot save - GameModel is null");
            return;
        }

        // Apply editable values to the game model
        gameModel.Coins = editableCoins;
        gameModel.Credits = editableCredits;
        gameModel.LifetimeDestructionScore = editableLifetimeDestructionScore;
        gameModel.HasAttemptedTutorial = editableHasAttemptedTutorial;

        // Apply additional editable values
        gameModel.PlayerName = editablePlayerName;
        gameModel.BattleWinScore = editableBattleWinScore;
        gameModel.TimesLostOnLastLevel = editableTimesLostOnLastLevel;
        gameModel.Bounty = editableBounty;
        gameModel.BestDestructionScore = editableBestDestructionScore;
        gameModel.PremiumEdition = editablePremiumEdition;
        gameModel.HasSyncdShop = editableHasSyncdShop;
        gameModel.GameMap = editableGameMap;
        gameModel.QueueName = editableQueueName;
        gameModel.ID_Bodykit_AIbot = editableID_Bodykit_AIbot;
        gameModel.SelectedPvPLevel = editableSelectedPvPLevel;

        // ApplicationModel properties automatically save when set
        ApplicationModel.SelectedLevel = editableSelectedLevel;
        ApplicationModel.SelectedSideQuestID = editableSelectedSideQuestID;

        // Save the game
        DataProvider.SaveGame();

        Debug.Log("Save game data has been updated and saved!");

        // Refresh the display
        LoadGameModel();
    }

    private void ApplySideQuestChanges()
    {
        if (gameModel == null)
        {
            Debug.LogError("Cannot apply side quest changes - GameModel is null");
            return;
        }

        // Warning: This is a workaround since CompletedSideQuests is ReadOnly
        // We need to manipulate the private field through reflection or create a new save
        Debug.LogWarning("Side quest editing requires creating a new save with modified quest completion status.");

        // For now, just add any newly selected side quests that aren't already completed
        for (int i = 0; i < sideQuestCompletion.Count; i++)
        {
            if (sideQuestCompletion[i] && !gameModel.IsSideQuestCompleted(i))
            {
                gameModel.AddCompletedSideQuest(new CompletedLevel(i, Difficulty.Normal));
            }
        }

        // Note: This approach can only ADD side quests, not remove them
        // To fully reset side quests, you would need to delete the save game and start fresh

        // Save the changes
        DataProvider.SaveGame();

        Debug.Log("Added new side quest completions! (Note: Cannot remove existing completions with current API)");

        // Refresh the display
        LoadGameModel();
    }

    private void ResetSaveData()
    {
        Debug.Log("Resetting all save data...");

        // Use DataProvider's reset method
        DataProvider.Reset();

        // Reinitialize DataProvider to create fresh save
        // Note: This might require restarting the editor or reloading the scene
        Debug.Log("Save data has been reset! You may need to restart the editor or reload to see changes.");

        // Try to reload the game model
        LoadGameModel();
    }
}