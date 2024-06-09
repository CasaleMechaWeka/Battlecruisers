using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class PlayerEconomyEditor : EditorWindow
{

    private string playerId;
    private string base64_key_secret;


    private string accessToken;

    private string statusField;


    private int coins;
    private int credits;


    private const string PROJECT_ID = "b979b535-b7aa-40c8-a624-6d3a3d92b920";


    //production environment ID
    private const string ENVIRONMENT_ID = "f030ef9b-63e1-46d6-abe0-cd0088943beb";

    //currency IDs
    private const string COIN_ID = "COIN";
    private const string CREDIT_ID = "CREDIT";


    [MenuItem("Tools/Player Economy Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerEconomyEditor>("Player Economy Editor");
        Debug.Log("Show Player Economy Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Economy Editor", EditorStyles.boldLabel);
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Service account secret", EditorStyles.boldLabel);
        base64_key_secret = EditorGUILayout.TextField("Service account secret", base64_key_secret);

        GUILayout.Space(20);
        EditorGUILayout.LabelField("PlayerId to update", EditorStyles.boldLabel);
        playerId = EditorGUILayout.TextField("Player ID", playerId);


        GUILayout.Space(20);

        EditorGUILayout.LabelField("AccessToken - Only refresh if this is empty, or if it has timedout.", EditorStyles.miniLabel);
        EditorGUI.BeginDisabledGroup(true);
        accessToken = EditorGUILayout.TextField("Access Token", accessToken);
        EditorGUI.EndDisabledGroup();


        if (GUILayout.Button("Generate AccessToken"))
        {
            GenerateAccessToken();
        }
        GUILayout.Space(30);

        EditorGUILayout.LabelField("User values", EditorStyles.boldLabel);
        GUILayout.Space(10);

        coins = EditorGUILayout.IntField("Coins", coins);
        credits = EditorGUILayout.IntField("Credits", credits);

        GUILayout.Space(10);
        if (GUILayout.Button("Load Player Data"))
        {
            LoadPlayerData();
        }
        EditorGUILayout.LabelField("Warning this will override the users values with what has been set in the textfields above.", EditorStyles.miniLabel);

        if (GUILayout.Button("Save Player Data"))
        {
            SavePlayerData();
        }

        EditorGUI.BeginDisabledGroup(true);
        statusField = EditorGUILayout.TextField("Status", statusField);
        EditorGUI.EndDisabledGroup();
    }

    private async void GenerateAccessToken()
    {
        string url = $"https://services.api.unity.com/auth/v1/token-exchange?projectId={PROJECT_ID}&environmentId={ENVIRONMENT_ID}";

        string AuthorizationHeader = $"Basic {base64_key_secret}";

        UnityWebRequest request = new(url, "POST");
        request.SetRequestHeader("Authorization", AuthorizationHeader);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();

        var asyncOperation = request.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // Deserialize the JSON response
            TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(responseText);
            accessToken = tokenResponse.accessToken;
            Debug.Log("Access Token: " + accessToken);
            statusField = "Access Token loaded.";
            Repaint();
        }
        else
        {
            statusField = "Error: " + request.error;

            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
    }

    private async void LoadPlayerData()
    {

        if (!ValidateInputFields()) return;

        string url = $"https://economy.services.api.unity.com/v2/projects/{PROJECT_ID}/players/{playerId}/currencies";

        string AuthorizationHeader = $"Bearer {accessToken}";

        UnityWebRequest request = new(url);
        request.SetRequestHeader("Authorization", AuthorizationHeader);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();

        var asyncOperation = request.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);
            // Deserialize the JSON response

            DataObject dataObject = JsonUtility.FromJson<DataObject>(responseText);
            foreach (ResultObject result in dataObject.results)
            {
                if (result.currencyId == COIN_ID)
                {
                    coins = result.balance;
                }
                else if (result.currencyId == CREDIT_ID)
                {
                    credits = result.balance;
                }
            }
            statusField = "Player data loaded";
            Repaint();
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);

            statusField = "Error: " + request.error;
            Repaint();
        }
    }

    private async void SavePlayerData()
    {
        if (!ValidateInputFields()) return;

        bool result = await SaveCurrency(COIN_ID, coins);
        if (result)
            result = await SaveCurrency(CREDIT_ID, credits);
    }

    private async Task<bool> SaveCurrency(string currencyId, int balance)
    {
        string url = $"https://economy.services.api.unity.com/v2/projects/{PROJECT_ID}/players/{playerId}/currencies/{currencyId}";
        string AuthorizationHeader = $"Bearer {accessToken}";

        Debug.Log(url);
        CurrencyBalanceRequest payload = new(currencyId, balance);
        // Serialize the payload to JSON
        string jsonPayload = JsonUtility.ToJson(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        UnityWebRequest request = new(url, "PUT");
        request.SetRequestHeader("Authorization", AuthorizationHeader);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        var asyncOperation = request.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"{currencyId} updated to value: {balance}");
            statusField = $"{currencyId} updated to value: {balance} SUCCEEDED";
            Repaint();
            return true;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);

            Debug.Log($"{currencyId} updated to value: {balance} FAILED");
            statusField = $"{currencyId} updated to value: {balance} FAILED";
            Repaint();
            return false;
        }
    }

    private bool ValidateInputFields()
    {
        if (string.IsNullOrEmpty(base64_key_secret))
        {
            Debug.LogError("key secret is empty.");
            return false;
        }
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("Player ID is empty.");
            return false;
        }
        if (string.IsNullOrEmpty(accessToken))
        {
            Debug.LogError("accessToken is empty.");
            return false;
        }
        return true;
    }

}

// User save request

[System.Serializable]
public class CurrencyBalanceRequest
{
    public string currencyId;
    public long balance;

    public CurrencyBalanceRequest(string currencyId, long balance)
    {
        this.currencyId = currencyId;
        this.balance = balance;
    }
}

//AccessToken Response

[System.Serializable]
public class TokenResponse
{
    public string accessToken;
}


//User currencies response

[System.Serializable]
public class DataObject
{
    public LinksObject links;
    public ResultObject[] results;
}

[System.Serializable]
public class LinksObject
{
    public string next;
}

[System.Serializable]
public class ResultObject
{
    public DateObject created;
    public DateObject modified;
    public int balance;
    public string currencyId;
    public string writeLock;
}

[System.Serializable]
public class DateObject
{
    public DateTime date;
}