using UnityEngine;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using System.Linq;
using BattleCruisers.Data.Static;
using UnityEditor;

public class PlayerSaveGameEditor : EditorWindow
{
    private GameModel gameModel;
    private ApplicationModel applicationModel;
    private List<bool> sideQuestCompletion;
    private int mainQuestsUnlocked;
    private string gameModelDataDisplay;

    [MenuItem("Tools/Player Save Game Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerSaveGameEditor>("Player Save Game Editor");
    }

    private void OnEnable()
    {
        LoadGameModel();
        sideQuestCompletion = new List<bool>(new bool[StaticData.SideQuests.Count]);
        gameModelDataDisplay = "";
    }

    private void OnGUI()
    {
        if (gameModel == null || applicationModel == null)
        {
            EditorGUILayout.HelpBox("GameModel or ApplicationModel is not available.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Load Latest Values"))
        {
            LoadGameModel();
        }

        EditorGUILayout.LabelField("Current Game State", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Main Quests Unlocked: {mainQuestsUnlocked}");
        EditorGUILayout.LabelField($"Completed Side Quests: {sideQuestCompletion.Count(sq => sq)}");
        EditorGUILayout.LabelField($"Coins: {gameModel.Coins}");
        EditorGUILayout.LabelField($"Credits: {gameModel.Credits}");
        EditorGUILayout.LabelField($"Tutorial Completed: {gameModel.HasAttemptedTutorial}");

        EditorGUILayout.LabelField("Application Model Details", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Selected Level: {applicationModel.SelectedLevel}");
        EditorGUILayout.LabelField($"Selected Side Quest ID: {applicationModel.SelectedSideQuestID}");
        EditorGUILayout.LabelField($"Game Mode: {applicationModel.Mode}");
        EditorGUILayout.LabelField($"Lifetime Destruction Score: {gameModel.LifetimeDestructionScore}");
        EditorGUILayout.LabelField($"Num of Levels Completed: {gameModel.NumOfLevelsCompleted}");

        EditorGUILayout.LabelField("Completed Side Quests:", EditorStyles.boldLabel);
        for (int i = 0; i < sideQuestCompletion.Count; i++)
        {
            if (sideQuestCompletion[i])
            {
                EditorGUILayout.LabelField($"Side Quest {i} is completed.");
            }
        }
    }

    private void LoadGameModel()
    {
        DataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;

        if (dataProvider == null || dataProvider.GameModel == null)
        {
            Debug.LogError("DataProvider or GameModel is not initialized.");
            return;
        }

        gameModel = dataProvider.GameModel as GameModel;
        applicationModel = ApplicationModelProvider.ApplicationModel as ApplicationModel;

        if (gameModel == null || applicationModel == null)
        {
            Debug.LogError("GameModel or ApplicationModel is not of type GameModel or ApplicationModel.");
            return;
        }
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
}