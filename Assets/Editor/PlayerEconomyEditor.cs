using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Newtonsoft.Json;

public class PlayerEconomyEditor : EditorWindow
{
    private string playerId;
    private int coins;
    private int credits;
    private bool isInitialized;

    [MenuItem("Tools/Player Economy Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerEconomyEditor>("Player Economy Editor");
    }

    private async void OnEnable()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError("Unity Services require the Editor to be in Play mode.");
            return;
        }

        await InitializeUnityServices();
    }

    private async Task InitializeUnityServices()
    {
        if (!isInitialized)
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                isInitialized = true;
                Debug.Log("Unity Services initialized and signed in.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to initialize Unity Services: " + ex.Message);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Economy Editor", EditorStyles.boldLabel);

        playerId = EditorGUILayout.TextField("Player ID", playerId);
        coins = EditorGUILayout.IntField("Coins", coins);
        credits = EditorGUILayout.IntField("Credits", credits);

        if (GUILayout.Button("Load Player Data"))
        {
            LoadPlayerData();
        }

        if (GUILayout.Button("Save Player Data"))
        {
            SavePlayerData();
        }
    }

    private async void LoadPlayerData()
    {
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("Player ID is empty.");
            return;
        }

        try
        {
            var playerData = await LoadUserData(playerId);
            if (playerData != null)
            {
                coins = playerData.Coins;
                credits = playerData.Credits;
                Repaint();
                Debug.Log($"Loaded data for Player ID: {playerId}");
            }
            else
            {
                Debug.LogError($"No data found for Player ID: {playerId}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading player data: " + ex.Message);
        }
    }

    private async void SavePlayerData()
    {
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("Player ID is empty.");
            return;
        }

        try
        {
            var playerData = new PlayerData { Coins = coins, Credits = credits };
            await SaveUserData(playerId, playerData);
            Debug.Log($"Saved data for Player ID: {playerId}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving player data: " + ex.Message);
        }
    }

    private async Task<PlayerData> LoadUserData(string userId)
    {
        try
        {
            // Ensure data is loaded with the correct key structure
            var response = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "player_" + userId });
            if (response.TryGetValue("player_" + userId, out var jsonData))
            {
                return JsonConvert.DeserializeObject<PlayerData>(jsonData);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading user data: " + ex.Message);
        }
        return null;
    }

    private async Task SaveUserData(string userId, PlayerData playerData)
    {
        try
        {
            var data = new Dictionary<string, object>
            {
                { "player_" + userId, JsonConvert.SerializeObject(playerData) }
            };

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving user data: " + ex.Message);
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public int Coins;
    public int Credits;
}
