using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System.Linq;
using System;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class PvPBattleSceneGodTunnel : NetworkBehaviour
    {
        public IPvPBattleCompletionHandler battleCompletionHandler;
        // left player
        private List<BuildingKey> _unlockedBuildings_LeftPlayer = new List<BuildingKey>();
        private List<UnitKey> _unlockedUnits_LeftPlayer = new List<UnitKey>();

        // right player
        private List<BuildingKey> _unlockedBuildings_RightPlayer = new List<BuildingKey>();
        private List<UnitKey> _unlockedUnits_RightPlayer = new List<UnitKey>();
        public int currentLevelNum;
        // events
        public NetworkVariable<Tunnel_BattleCompletedState> BattleCompleted = new NetworkVariable<Tunnel_BattleCompletedState>(writePerm: NetworkVariableWritePermission.Owner); // _battleCompletionHandler.BattleCompleted?.

        public Action RegisteredAllUnlockedBuildables;
        private bool IsRegisteredBuildablesLeftPlayer;
        private bool IsRegisteredBuildablesRightPlayer;

        public static float _levelTimeInSeconds;
        public static long _aircraftVal;
        public static long _shipsVal;
        public static long _cruiserVal;
        public static long _buildingsVal;
        public static long[] _totalDestroyed;
        public static string _enemyCruiserName;

        /// <summary>
        ///  need to hold all values here to handle player disconnection
        /// </summary>
        /// 
        public static float _playerALevelTimeInSeconds;
        public static long _playerAAircraftVal;
        public static long _playerAShipsVal;
        public static long _playerACruiserVal;
        public static long _playerABuildingsVal;
        public static long[] _playerATotoalDestroyed = new long[4];
        public static string _playerACruiserName;

        public static float _playerBLevelTimeInSeconds;
        public static long _playerBAircraftVal;
        public static long _playerBShipsVal;
        public static long _playerBCruiserVal;
        public static long _playerBBuildingsVal;
        public static long[] _playerBTotoalDestroyed = new long[4];
        public static string _playerBCruiserName;

        public static bool OpponentQuit = false;
        public static int isDisconnected = 0;
        public static bool isCost = false;
        public static float difficultyDestructionScoreMultiplier = 1.0f;
        public static Dictionary<string, long> cruiser_scores = new Dictionary<string, long>() {
            { "BlackRig", 5800},
            { "Bullshark", 4000},
            { "Eagle", 2400},
            { "Flea", 4000},
            { "Goatherd", 5700},
            { "Hammerhead", 3900},
            { "HuntressBoss", 150000},
            { "Longbow", 5000},
            { "ManOfWarBoss", 10000},
            { "Megalodon", 5000},
            { "Megalith", 7000},
            { "Microlodon", 1500},
            { "Pistol", 2200},
            { "Raptor", 1500},
            { "Rickshaw", 1500},
            { "Rockjaw", 3600},
            { "Shepherd", 4000},
            { "TasDevil", 6800},
            { "Trident", 3500},
            { "Yeti", 8000},
        };
        private void Awake()
        {
            isDisconnected = 0;
            _playerALevelTimeInSeconds = 1;
            _playerAAircraftVal = 0;
            _playerAShipsVal = 0;
            //    _playerACruiserVal = 3500;
            _playerABuildingsVal = 0;
            _playerATotoalDestroyed = new long[4];
            //    _playerACruiserName = "";

            _playerBLevelTimeInSeconds = 1;
            _playerBAircraftVal = 0;
            _playerBShipsVal = 0;
            //    _playerBCruiserVal = 3500;
            _playerBBuildingsVal = 0;
            _playerBTotoalDestroyed = new long[4];
            //    _playerBCruiserName = "";


            if (DataProvider.SettingsManager.AIDifficulty == Difficulty.Normal)
            {
                difficultyDestructionScoreMultiplier = 1.0f;
            }
            if (DataProvider.SettingsManager.AIDifficulty == Difficulty.Hard)
            {
                difficultyDestructionScoreMultiplier = 1.5f;
            }
            if (DataProvider.SettingsManager.AIDifficulty == Difficulty.Harder)
            {
                difficultyDestructionScoreMultiplier = 2.0f;
            }
        }

        public static void AddAllBuildablesOfLeftPlayer(TargetType type, float val)
        {
            switch (type)
            {
                case TargetType.Aircraft:
                    _playerAAircraftVal += (long)val;
                    _playerATotoalDestroyed[0]++;
                    break;
                case TargetType.Ships:
                    _playerAShipsVal += (long)val;
                    _playerATotoalDestroyed[1]++;
                    break;
                case TargetType.Cruiser:
                    _playerACruiserVal += (long)val;
                    _playerATotoalDestroyed[2]++;
                    break;
                case TargetType.Buildings:
                    _playerABuildingsVal += (long)val;
                    _playerATotoalDestroyed[3]++;
                    break;
            }
        }

        public static void AddAllBuildablesOfRightPlayer(TargetType type, float val)
        {
            switch (type)
            {
                case TargetType.Aircraft:
                    _playerBAircraftVal += (long)val;
                    _playerBTotoalDestroyed[0]++;
                    break;
                case TargetType.Ships:
                    _playerBShipsVal += (long)val;
                    _playerBTotoalDestroyed[1]++;
                    break;
                case TargetType.Cruiser:
                    _playerBCruiserVal += (long)val;
                    _playerBTotoalDestroyed[2]++;
                    break;
                case TargetType.Buildings:
                    _playerBBuildingsVal += (long)val;
                    _playerBTotoalDestroyed[3]++;
                    break;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                BattleCompleted.Value = Tunnel_BattleCompletedState.None;
            IsRegisteredBuildablesLeftPlayer = false;
            IsRegisteredBuildablesRightPlayer = false;
        }

        public void HandleCruiserDestroyed()
        {
            HandleCruiserDestroyedClientRpc();
        }

        public void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            if (IsServer)
            {
                Debug.Log($"SERVER Complete Battle is Victory {wasVictory}");

                //handle server stats
                _levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_left[TargetType.PlayedTime].GetPlayedTime();
                _aircraftVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Aircraft].GetTotalDamageInCredits();
                _shipsVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Ships].GetTotalDamageInCredits();
                _cruiserVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Cruiser].GetTotalDamageInCredits();
                _buildingsVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Buildings].GetTotalDamageInCredits();
                _enemyCruiserName = PvPBattleSceneGodServer.playerBCruiserName;
                _totalDestroyed = new long[4];
                for (int i = 0; i < 4; i++)
                {
                    _totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_left[(TargetType)i].GetTotalDestroyed();
                }
                Debug.Log($"CRUISERS DESTROYED LEFT: {_totalDestroyed[2]}");
                battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel);

                //handle client stats
                float levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_right[TargetType.PlayedTime].GetPlayedTime();
                long aircraftVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Aircraft].GetTotalDamageInCredits();
                long shipsVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Ships].GetTotalDamageInCredits();
                long cruiserVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Cruiser].GetTotalDamageInCredits();
                long buildingsVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Buildings].GetTotalDamageInCredits();
                string enemyCruiserName = PvPBattleSceneGodServer.enemyCruiserName;
                long[] totalDestroyed = new long[4];
                for (int i = 0; i < 4; i++)
                {
                    totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_right[(TargetType)i].GetTotalDestroyed();
                }
                Debug.Log($"CRUISERS DESTROYED RIGHT: {totalDestroyed[2]}");
                CompleteBattleClientRpc(!wasVictory, retryLevel, levelTimeInSeconds, aircraftVal, shipsVal, cruiserVal, buildingsVal, enemyCruiserName, totalDestroyed);

            }
            else
            {
                Debug.Log("Client Complete Battle");
            }
        }

        public void ChangeBattleCompletedValue(Tunnel_BattleCompletedState val)
        {
            if (IsClient)
            {
                ChangeBattleCompletedValueServerRpc(val);
            }
        }

        public void CompleteBattle(bool wasPlayerVictory, bool retryLevel, long destructionScore)
        {
            if (IsServer)
            {
                Debug.Log($"SERVER Complete Battle is Victory {wasPlayerVictory}");

                //handle server stats
                _levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_left[TargetType.PlayedTime].GetPlayedTime();
                _aircraftVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Aircraft].GetTotalDamageInCredits();
                _shipsVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Ships].GetTotalDamageInCredits();
                _cruiserVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Cruiser].GetTotalDamageInCredits();
                _buildingsVal = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Buildings].GetTotalDamageInCredits();
                _enemyCruiserName = PvPBattleSceneGodServer.playerBCruiserName;
                _totalDestroyed = new long[4];
                for (int i = 0; i < 4; i++)
                {
                    _totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_left[(TargetType)i].GetTotalDestroyed();
                }
                Debug.Log($"CRUISERS DESTROYED LEFT: {_totalDestroyed[2]}");

                battleCompletionHandler?.CompleteBattle(wasPlayerVictory, retryLevel, destructionScore);

                //handle client stats
                float levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_right[TargetType.PlayedTime].GetPlayedTime();
                long aircraftVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Aircraft].GetTotalDamageInCredits();
                long shipsVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Ships].GetTotalDamageInCredits();
                long cruiserVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Cruiser].GetTotalDamageInCredits();
                long buildingsVal = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Buildings].GetTotalDamageInCredits();
                string enemyCruiserName = PvPBattleSceneGodServer.enemyCruiserName;
                long[] totalDestroyed = new long[4];
                for (int i = 0; i < 4; i++)
                {
                    totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_right[(TargetType)i].GetTotalDestroyed();
                }
                Debug.Log($"CRUISERS DESTROYED RIGHT: {totalDestroyed[2]}");
                CompleteBattleClientRpc(!wasPlayerVictory, retryLevel, destructionScore, levelTimeInSeconds, aircraftVal, shipsVal, cruiserVal, buildingsVal, enemyCruiserName, totalDestroyed);
            }
            else
            {
                Debug.Log("Client Complete Battle");
            }
        }

        public void AddUnlockedBuilding_LeftPlayer(BuildingCategory category, string prefabName)
        {
            if (IsClient)
                AddUnlockedBuildingLeftPlayerServerRpc(category, prefabName);
        }

        public void AddUnlockedUnit_LeftPlayer(UnitCategory category, string prefabName)
        {
            if (IsClient)
                AddUnlockedUnitLeftPlayerServerRpc(category, prefabName);
        }

        public void AddUnlockedBuilding_RightPlayer(BuildingCategory category, string prefabName)
        {
            if (IsClient)
                AddUnlockedBuildingRightPlayerServerRpc(category, prefabName);
        }

        public void AddUnlockedBuilding_RightPlayer(BuildingKey key)
        {
            _unlockedBuildings_RightPlayer.Add(key);
        }

        public void AddUnlockedUnit_RightPlayer(UnitCategory category, string prefabName)
        {
            if (IsClient)
                AddUnlockedUnitRightPlayerServerRpc(category, prefabName);
        }

        public void AddUnlockedUnit_RightPlayer(UnitKey key)
        {
            _unlockedUnits_RightPlayer.Add(key);
        }

        public IList<PvPBuildingKey> GetUnlockedBuildings_LeftPlayer(BuildingCategory pvpBuildingCategory)
        {
            BuildingCategory buildingCategory = convertPvPBuildingCategory2PvEBuildingCategory(pvpBuildingCategory);
            IList<BuildingKey> iList = _unlockedBuildings_LeftPlayer.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
            IList<PvPBuildingKey> iPvPList = new List<PvPBuildingKey>();
            foreach (BuildingKey buildingKey in iList)
            {
                iPvPList.Add(new PvPBuildingKey(convertPvEBuildingCategory2PvPBuildingCategory(buildingKey.BuildingCategory), "PvP" + buildingKey.PrefabName));
            }

            return iPvPList;
        }

        public bool IsBuildingUnlocked_LeftPlayer(PvPBuildingKey buildingKey)
        {
            BuildingKey _buildingKey = convert2PvEBuildingKey(buildingKey);
            return _unlockedBuildings_LeftPlayer.Contains(_buildingKey);
        }


        public IList<PvPBuildingKey> GetUnlockedBuildings_RightPlayer(BuildingCategory pvpBuildingCategory)
        {
            BuildingCategory buildingCategory = convertPvPBuildingCategory2PvEBuildingCategory(pvpBuildingCategory);
            IList<BuildingKey> iList = _unlockedBuildings_RightPlayer.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
            IList<PvPBuildingKey> iPvPList = new List<PvPBuildingKey>();
            foreach (BuildingKey buildingKey in iList)
            {
                iPvPList.Add(new PvPBuildingKey(convertPvEBuildingCategory2PvPBuildingCategory(buildingKey.BuildingCategory), "PvP" + buildingKey.PrefabName));
            }

            return iPvPList;
        }

        public bool IsBuildingUnlocked_RightPlayer(PvPBuildingKey buildingKey)
        {
            BuildingKey _buildingKey = convert2PvEBuildingKey(buildingKey);
            return _unlockedBuildings_RightPlayer.Contains(_buildingKey);
        }

        private BuildingKey convert2PvEBuildingKey(PvPBuildingKey buildingKey)
        {
            return new BuildingKey(convertPvPBuildingCategory2PvEBuildingCategory(buildingKey.BuildingCategory), buildingKey.PrefabName.Remove(0, 3));
        }
        public IList<PvPUnitKey> GetUnlockedUnits_LeftPlayer(UnitCategory pvpUnitCategory)
        {
            UnitCategory unitCategory = convertPvPUnitCategory2PvEUnitCategory(pvpUnitCategory);
            IList<UnitKey> iList = _unlockedUnits_LeftPlayer.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
            IList<PvPUnitKey> iPvPList = new List<PvPUnitKey>();
            foreach (UnitKey unitKey in iList)
            {
                iPvPList.Add(new PvPUnitKey(convertPvEUnitCategory2PvPUnitCategory(unitKey.UnitCategory), "PvP" + unitKey.PrefabName));
            }
            return iPvPList;
        }

        public bool IsUnitUnlocked_LeftPlayer(PvPUnitKey unitKey)
        {
            UnitKey _unitKey = convert2PvEUnitKey(unitKey);
            return _unlockedUnits_LeftPlayer.Contains(_unitKey);
        }

        private UnitKey convert2PvEUnitKey(PvPUnitKey unitKey)
        {
            return new UnitKey(convertPvPUnitCategory2PvEUnitCategory(unitKey.UnitCategory), unitKey.PrefabName.Remove(0, 3));
        }

        public IList<PvPUnitKey> GetUnlockedUnits_RightPlayer(UnitCategory pvpUnitCategory)
        {
            UnitCategory unitCategory = convertPvPUnitCategory2PvEUnitCategory(pvpUnitCategory);
            IList<UnitKey> iList = _unlockedUnits_RightPlayer.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
            IList<PvPUnitKey> iPvPList = new List<PvPUnitKey>();
            foreach (UnitKey unitKey in iList)
            {
                iPvPList.Add(new PvPUnitKey(convertPvEUnitCategory2PvPUnitCategory(unitKey.UnitCategory), "PvP" + unitKey.PrefabName));
            }
            return iPvPList;
        }

        public bool IsUnitUnlocked_RightPlayer(PvPUnitKey unitKey)
        {
            UnitKey _unitKey = convert2PvEUnitKey(unitKey);
            return _unlockedUnits_RightPlayer.Contains(_unitKey);
        }

        public void RegisteredAllBuildableLeftPlayer()
        {
            RegisteredUnlockedBuildablesLeftPlayerServerRpc();
        }

        public void RegisteredAllBuildableRightPlayer()
        {
            RegisterUnlockedBuildablesRightPlayerServerRpc();
        }

        public void RegisteredAllBuildableAIPlayer()
        {
            RegisterUnlockedBuildablesAIPlayerServerRpc();
        }

        private UnitCategory convertPvPUnitCategory2PvEUnitCategory(UnitCategory category)
        {
            switch (category)
            {
                case UnitCategory.Aircraft: return UnitCategory.Aircraft;
                case UnitCategory.Naval: return UnitCategory.Naval;
                default: throw new System.Exception();
            }
        }

        private UnitCategory convertPvEUnitCategory2PvPUnitCategory(UnitCategory category)
        {
            switch (category)
            {
                case UnitCategory.Aircraft: return UnitCategory.Aircraft;
                case UnitCategory.Naval: return UnitCategory.Naval;
                default: throw new System.Exception();
            }
        }

        private BuildingCategory convertPvPBuildingCategory2PvEBuildingCategory(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Ultra:
                    return BuildingCategory.Ultra;
                case BuildingCategory.Tactical:
                    return BuildingCategory.Tactical;
                case BuildingCategory.Factory:
                    return BuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return BuildingCategory.Offence;
                case BuildingCategory.Defence:
                    return BuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
        }

        private BuildingCategory convertPvEBuildingCategory2PvPBuildingCategory(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Ultra:
                    return BuildingCategory.Ultra;
                case BuildingCategory.Tactical:
                    return BuildingCategory.Tactical;
                case BuildingCategory.Factory:
                    return BuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return BuildingCategory.Offence;
                case BuildingCategory.Defence:
                    return BuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
        }
        [ClientRpc]
        private void CompleteBattleClientRpc(bool wasVictory, bool retryLevel, float levelTimeInSeconds, long aircraftVal, long shipsVal, long cruiserVal, long buildingsVal, string enemyCruiserName, long[] totalDestroyed)
        {
            if (!IsServer)
            {
                Debug.Log($"Complete Battle Client RPC WasVictroy: {wasVictory} LevelTime: {levelTimeInSeconds}");
                _levelTimeInSeconds = levelTimeInSeconds;
                _aircraftVal = aircraftVal;
                _shipsVal = shipsVal;
                _cruiserVal = cruiserVal;
                _buildingsVal = buildingsVal;
                _enemyCruiserName = enemyCruiserName;
                _totalDestroyed = totalDestroyed;
                battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel);
            }
        }

        [ClientRpc]
        private void CompleteBattleClientRpc(bool wasVictory, bool retryLevel, long destructionScore, float levelTimeInSeconds, long aircraftVal, long shipsVal, long cruiserVal, long buildingsVal, string enemyCruiserName, long[] totalDestroyed)
        {
            if (!IsServer)
            {
                Debug.Log($"Complete Battle Client RPC WasVictroy: {wasVictory} destructionScore {destructionScore} LevelTime: {levelTimeInSeconds}");
                _levelTimeInSeconds = levelTimeInSeconds;
                _aircraftVal = aircraftVal;
                _shipsVal = shipsVal;
                _cruiserVal = cruiserVal;
                _buildingsVal = buildingsVal;
                _enemyCruiserName = enemyCruiserName;
                _totalDestroyed = totalDestroyed;
                battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel, destructionScore);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddUnlockedBuildingLeftPlayerServerRpc(BuildingCategory category, string prefabName)
        {
            _unlockedBuildings_LeftPlayer.Add(new BuildingKey(category, prefabName));
        }


        [ServerRpc(RequireOwnership = false)]
        private void AddUnlockedUnitLeftPlayerServerRpc(UnitCategory category, string prefabName)
        {
            _unlockedUnits_LeftPlayer.Add(new UnitKey(category, prefabName));
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddUnlockedBuildingRightPlayerServerRpc(BuildingCategory category, string prefabName)
        {
            _unlockedBuildings_RightPlayer.Add(new BuildingKey(category, prefabName));
        }


        [ServerRpc(RequireOwnership = false)]
        private void AddUnlockedUnitRightPlayerServerRpc(UnitCategory category, string prefabName)
        {
            _unlockedUnits_RightPlayer.Add(new UnitKey(category, prefabName));
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisteredUnlockedBuildablesLeftPlayerServerRpc()
        {
            IsRegisteredBuildablesLeftPlayer = true;
            if (IsRegisteredBuildablesRightPlayer)
                RegisteredAllUnlockedBuildables?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisterUnlockedBuildablesRightPlayerServerRpc()
        {
            IsRegisteredBuildablesRightPlayer = true;
            if (IsRegisteredBuildablesLeftPlayer)
                RegisteredAllUnlockedBuildables?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisterUnlockedBuildablesAIPlayerServerRpc()
        {
            RegisteredAllUnlockedBuildables?.Invoke();
        }

        [ClientRpc]
        private void HandleCruiserDestroyedClientRpc()
        {
            PvPBattleSceneGodClient.Instance.HandleCruiserDestroyed();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeBattleCompletedValueServerRpc(Tunnel_BattleCompletedState val)
        {
            BattleCompleted.Value = val;
        }
    }

    public enum Tunnel_BattleCompletedState
    {
        None = 0,
        Completed = 1
    }
}
