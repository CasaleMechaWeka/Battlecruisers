using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public enum Map
    {
        PracticeWreckyards,
        OzPenitentiary,
        SanFranciscoFightClub,
        UACBattleNight,
        NuclearDome,
        UACArena,
        RioBattlesport,
        UACUltimate,
        MercenaryOne
    }


    public enum GameMode
    {
        Starting,
        Gambling
    }

    public enum GameQueue
    {
        Casual,
        Competitive
    }


    public class MatchplayUser
    {
        public MatchplayUser()
        {
            var tmepId = Guid.NewGuid().ToString();
            Data = new UserData(NameGenerator.GetName(tmepId), tmepId, 0, new GameInfo());
        }

        public UserData Data { get; }

        public string Name
        {
            get => Data.userName;
            set
            {
                Data.userName = value;
                onNameChanged?.Invoke(Data.userName);
            }
        }


        public Action<string> onNameChanged;

        public string AuthId
        {
            get => Data.userAuthId;
            set => Data.userAuthId = value;
        }


        public Map MapPreferences
        {
            get => Data.userGamePreferences.map;
            set { Data.userGamePreferences.map = value; }
        }


        public GameMode GameModePreferences
        {
            get => Data.userGamePreferences.gameMode;
            set => Data.userGamePreferences.gameMode = value;
        }

        public GameQueue QueuePreference
        {
            get => Data.userGamePreferences.gameQueue;
            set => Data.userGamePreferences.gameQueue = value;
        }

        public override string ToString()
        {
            var userData = new StringBuilder("MatchplayerUser: ");
            userData.AppendLine($"- {Data}");
            return userData.ToString();
        }
    }


    [Serializable]
    public class UserData
    {
        public string userName;
        public string userAuthId;
        public ulong networkId;
        public GameInfo userGamePreferences;
        public UserData(string userName, string userAuthId, ulong networkId, GameInfo userGamePreferences)
        {
            this.userName = userName;
            this.userAuthId = userAuthId;
            this.networkId = networkId;
            this.userGamePreferences = userGamePreferences;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UserData: ");
            sb.AppendLine($"- User Name:                    {userName}");
            sb.AppendLine($"- User Auth Id:                 {userAuthId}");
            sb.AppendLine($"- User Game Preferences:        {userGamePreferences}");
            return sb.ToString();
        }

    }

    [Serializable]
    public class GameInfo
    {
        public Map map;
        public GameMode gameMode;
        public GameQueue gameQueue;

        public int MaxUsers = 3;
        public string ToSceneName => ConvertToScene(map);

        const string k_MultiplayCasualQueue = "casual-queue";
        const string k_MultiplayCompetetiveQueue = "competetive-queue";
        static readonly Dictionary<string, GameQueue> k_MultiplayToLocalQueueNames = new Dictionary<string, GameQueue>
        {
            { k_MultiplayCasualQueue, GameQueue.Casual },
            { k_MultiplayCompetetiveQueue, GameQueue.Competitive }
        };


        public Dictionary<string, DataObject> GetDataForUnityServices() =>
            new Dictionary<string, DataObject>()
            {
                        {
                            "Map", new DataObject(
                            visibility: DataObject.VisibilityOptions.Public,
                            value: ToSceneName,
                            index: DataObject.IndexOptions.S1)
                        },
            };

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GameInfo: ");
            sb.AppendLine($"- map:           {map}");
            sb.AppendLine($"- gameMode:      {gameMode}");
            sb.AppendLine($"- gameQueue:     {gameQueue}");
            return sb.ToString();
        }
        public static string ConvertToScene(Map map)
        {
            switch (map)
            {
                case Map.PracticeWreckyards:
                    return "PracticeWreckyards";
                case Map.OzPenitentiary:
                    return "OzPenitentiary";
                case Map.SanFranciscoFightClub:
                    return "SanFranciscoFightClub";
                case Map.UACBattleNight:
                    return "UACBattleNight";
                case Map.UACArena:
                    return "UACArena";
                case Map.RioBattlesport:
                    return "RioBattlesport";
                case Map.UACUltimate:
                    return "UACUltimate";
                case Map.MercenaryOne:
                    return "MercenaryOne";
                default:
                    return "Non exist";
            }
        }
        public string ToMultiplayQueue()
        {
            return gameQueue switch
            {
                GameQueue.Casual => k_MultiplayCasualQueue,
                GameQueue.Competitive => k_MultiplayCompetetiveQueue,
                _ => k_MultiplayCasualQueue
            };
        }


        public static GameQueue ToGameQueue(string multiplayQueue)
        {
            if (!k_MultiplayToLocalQueueNames.ContainsKey(multiplayQueue))
            {
                Debug.LogWarning($"No QueuePreference that maps to  {multiplayQueue}");
                return GameQueue.Casual;
            }
            return k_MultiplayToLocalQueueNames[multiplayQueue];
        }
    }


}

