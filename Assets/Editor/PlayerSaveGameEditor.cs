using UnityEngine;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System.Linq;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using UnityEditor;

public class PlayerSaveGameEditor : EditorWindow
{
    private GameModel gameModel;
    private List<bool> sideQuestCompletion;
    private int mainQuestsUnlocked;
    private string gameModelDataDisplay;
    private StaticData staticData;

    [MenuItem("Tools/Player Save Game Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerSaveGameEditor>("Player Save Game Editor");
    }

    private void OnEnable()
    {
        staticData = new StaticData();
        LoadGameModel();
        sideQuestCompletion = new List<bool>(new bool[staticData.SideQuests.Count]);
        gameModelDataDisplay = "";
    }

    private void OnGUI()
    {
        if (gameModel == null)
        {
            EditorGUILayout.HelpBox("GameModel is not available.", MessageType.Warning);
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

        EditorGUILayout.LabelField("Completed Side Quests:", EditorStyles.boldLabel);
        for (int i = 0; i < sideQuestCompletion.Count; i++)
        {
            if (sideQuestCompletion[i])
            {
                EditorGUILayout.LabelField($"Side Quest {i + 1} is completed.");
            }
        }
    }

    private void LoadGameModel()
    {
        IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;

        if (dataProvider == null || dataProvider.GameModel == null)
        {
            Debug.LogError("DataProvider or GameModel is not initialized.");
            return;
        }

        gameModel = dataProvider.GameModel as GameModel;
        if (gameModel == null)
        {
            Debug.LogError("GameModel is not of type GameModel.");
            return;
        }
        mainQuestsUnlocked = gameModel.CompletedLevels.Count;
        sideQuestCompletion = new List<bool>(new bool[staticData.SideQuests.Count]);

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