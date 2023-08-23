using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System.Linq;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.PostBattleScreen;

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



        public override void OnNetworkSpawn()
        {
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
                if(wasVictory)
                {
                    float levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.PlayedTime].GetPlayedTime();
                    long aircraftVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Aircraft].GetTotalDamageInCredits();
                    long shipsVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Ships].GetTotalDamageInCredits();
                    long cruiserVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Cruiser].GetTotalDamageInCredits();
                    long buildingsVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Buildings].GetTotalDamageInCredits();
                    long[] totalDestroyed = new long[4];
                    for (int i = 0; i < 4; i++)
                    {

                        totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_left[(PvPTargetType)i].GetTotalDestroyed();
                    }
                    string enemyCruiserName = PvPBattleSceneGodServer.enemyCruiserName;
                    CompleteBattleClientRpc(wasVictory, retryLevel, levelTimeInSeconds, aircraftVal, shipsVal, cruiserVal, buildingsVal, enemyCruiserName, totalDestroyed);
                }
                else
                {
                    float levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.PlayedTime].GetPlayedTime();
                    long aircraftVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Aircraft].GetTotalDamageInCredits();
                    long shipsVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Ships].GetTotalDamageInCredits();
                    long cruiserVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Cruiser].GetTotalDamageInCredits();
                    long buildingsVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Buildings].GetTotalDamageInCredits();
                    long[] totalDestroyed = new long[4];
                    for (int i = 0; i < 4; i++)
                    {

                        totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_right[(PvPTargetType)i].GetTotalDestroyed();
                    }
                    string enemyCruiserName = PvPBattleSceneGodServer.enemyCruiserName;
                    CompleteBattleClientRpc(wasVictory, retryLevel, levelTimeInSeconds, aircraftVal, shipsVal, cruiserVal, buildingsVal, enemyCruiserName, totalDestroyed);
                }
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
                if(wasPlayerVictory)
                {
                    float levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.PlayedTime].GetPlayedTime();
                    long aircraftVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Aircraft].GetTotalDamageInCredits();
                    long shipsVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Ships].GetTotalDamageInCredits();
                    long cruiserVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Cruiser].GetTotalDamageInCredits();
                    long buildingsVal = PvPBattleSceneGodServer.deadBuildables_left[Buildables.PvPTargetType.Buildings].GetTotalDamageInCredits();
                    string enemyCruiserName = PvPBattleSceneGodServer.enemyCruiserName;
                    long[] totalDestroyed = new long[4];
                    for (int i = 0; i < 4; i++)
                    {

                        totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_left[(PvPTargetType)i].GetTotalDestroyed();
                    }
                    CompleteBattleClientRpc(wasPlayerVictory, retryLevel, destructionScore, levelTimeInSeconds, aircraftVal, shipsVal, cruiserVal, buildingsVal, enemyCruiserName, totalDestroyed);
                }
                else
                {
                    float levelTimeInSeconds = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.PlayedTime].GetPlayedTime();
                    long aircraftVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Aircraft].GetTotalDamageInCredits();
                    long shipsVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Ships].GetTotalDamageInCredits();
                    long cruiserVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Cruiser].GetTotalDamageInCredits();
                    long buildingsVal = PvPBattleSceneGodServer.deadBuildables_right[Buildables.PvPTargetType.Buildings].GetTotalDamageInCredits();
                    string enemyCruiserName = PvPBattleSceneGodServer.enemyCruiserName;
                    long[] totalDestroyed = new long[4];
                    for (int i = 0; i < 4; i++)
                    {

                        totalDestroyed[i] = PvPBattleSceneGodServer.deadBuildables_right[(PvPTargetType)i].GetTotalDestroyed();
                    }
                    CompleteBattleClientRpc(wasPlayerVictory, retryLevel, destructionScore, levelTimeInSeconds, aircraftVal, shipsVal, cruiserVal, buildingsVal, enemyCruiserName, totalDestroyed);
                }
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

        public void AddUnlockedUnit_RightPlayer(UnitCategory category, string prefabName)
        {
            if (IsClient)
                AddUnlockedUnitRightPlayerServerRpc(category, prefabName);
        }

        public IList<PvPBuildingKey> GetUnlockedBuildings_LeftPlayer(PvPBuildingCategory pvpBuildingCategory)
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


        public IList<PvPBuildingKey> GetUnlockedBuildings_RightPlayer(PvPBuildingCategory pvpBuildingCategory)
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
        public IList<PvPUnitKey> GetUnlockedUnits_LeftPlayer(PvPUnitCategory pvpUnitCategory)
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

        public IList<PvPUnitKey> GetUnlockedUnits_RightPlayer(PvPUnitCategory pvpUnitCategory)
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

        private UnitCategory convertPvPUnitCategory2PvEUnitCategory(PvPUnitCategory category)
        {
            switch (category)
            {
                case PvPUnitCategory.Aircraft: return UnitCategory.Aircraft;
                case PvPUnitCategory.Naval: return UnitCategory.Naval;
                default: throw new System.Exception();
            }
        }

        private PvPUnitCategory convertPvEUnitCategory2PvPUnitCategory(UnitCategory category)
        {
            switch (category)
            {
                case UnitCategory.Aircraft: return PvPUnitCategory.Aircraft;
                case UnitCategory.Naval: return PvPUnitCategory.Naval;
                default: throw new System.Exception();
            }
        }

        private BuildingCategory convertPvPBuildingCategory2PvEBuildingCategory(PvPBuildingCategory category)
        {
            switch (category)
            {
                case PvPBuildingCategory.Ultra:
                    return BuildingCategory.Ultra;
                case PvPBuildingCategory.Tactical:
                    return BuildingCategory.Tactical;
                case PvPBuildingCategory.Factory:
                    return BuildingCategory.Factory;
                case PvPBuildingCategory.Offence:
                    return BuildingCategory.Offence;
                case PvPBuildingCategory.Defence:
                    return BuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
        }

        private PvPBuildingCategory convertPvEBuildingCategory2PvPBuildingCategory(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Ultra:
                    return PvPBuildingCategory.Ultra;
                case BuildingCategory.Tactical:
                    return PvPBuildingCategory.Tactical;
                case BuildingCategory.Factory:
                    return PvPBuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return PvPBuildingCategory.Offence;
                case BuildingCategory.Defence:
                    return PvPBuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
        }
        [ClientRpc]
        private void CompleteBattleClientRpc(bool wasVictory, bool retryLevel, float levelTimeInSeconds, long aircraftVal, long shipsVal, long cruiserVal, long buildingsVal, string enemyCruiserName, long[] totalDestroyed)
        {
            _levelTimeInSeconds = levelTimeInSeconds;
            _aircraftVal = aircraftVal;
            _shipsVal = shipsVal;
            _cruiserVal = cruiserVal;
            _buildingsVal = buildingsVal;
            _enemyCruiserName = enemyCruiserName;
            _totalDestroyed = totalDestroyed;
            battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel);
        }

        [ClientRpc]
        private void CompleteBattleClientRpc(bool wasVictory, bool retryLevel, long destructionScore, float levelTimeInSeconds, long aircraftVal, long shipsVal, long cruiserVal, long buildingsVal, string enemyCruiserName, long[] totalDestroyed)
        {
            _levelTimeInSeconds = levelTimeInSeconds;
            _aircraftVal = aircraftVal;
            _shipsVal = shipsVal;
            _cruiserVal = cruiserVal;
            _buildingsVal = buildingsVal;
            _enemyCruiserName = enemyCruiserName;
            _totalDestroyed = totalDestroyed;
            battleCompletionHandler?.CompleteBattle(wasVictory, retryLevel, destructionScore);
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
