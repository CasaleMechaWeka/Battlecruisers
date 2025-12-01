using System;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using System.Collections.Generic;
using BattleCruisers.Utils;
using BattleCruisers.Buildables;
using BattleCruisers.Scenes.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class SynchedServerData : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<NetworkString> serverID = new NetworkVariable<NetworkString>();
        public NetworkVariable<Map> map = new NetworkVariable<Map>();
        public NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>();
        public NetworkVariable<GameQueue> gameQueue = new NetworkVariable<GameQueue>();

        public NetworkVariable<int> playerACruiserID = new NetworkVariable<int>();
        public NetworkVariable<ulong> playerAClientNetworkId = new NetworkVariable<ulong>();
        public NetworkVariable<NetworkString> playerAName = new NetworkVariable<NetworkString>();
        public NetworkVariable<long> playerAScore = new NetworkVariable<long>();
        public NetworkVariable<NetworkString> captainAPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<float> playerARating = new NetworkVariable<float>();
        public NetworkVariable<int> playerABodykit = new NetworkVariable<int>();
        public NetworkVariable<int> playerABounty = new NetworkVariable<int>();
        //    public NetworkVariable<NetworkString> playerASelectedVariants = new NetworkVariable<NetworkString>();


        public NetworkVariable<int> playerBCruiserID = new NetworkVariable<int>();
        public NetworkVariable<ulong> playerBClientNetworkId = new NetworkVariable<ulong>();
        public NetworkVariable<NetworkString> playerBName = new NetworkVariable<NetworkString>();
        public NetworkVariable<long> playerBScore = new NetworkVariable<long>();
        public NetworkVariable<NetworkString> captainBPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<float> playerBRating = new NetworkVariable<float>();
        public NetworkVariable<int> playerBBodykit = new NetworkVariable<int>();
        public NetworkVariable<int> playerBBounty = new NetworkVariable<int>();
        //    public NetworkVariable<NetworkString> playerBSelectedVariants = new NetworkVariable<NetworkString>();

        public NetworkVariable<float> left_levelTimeInSeconds = new NetworkVariable<float>();
        public NetworkVariable<long> left_aircraftVal = new NetworkVariable<long>();
        public NetworkVariable<long> left_shipsVal = new NetworkVariable<long>();
        public NetworkVariable<long> left_cruiserVal = new NetworkVariable<long>();
        public NetworkVariable<long> left_buildingsVal = new NetworkVariable<long>();
        public NetworkVariable<NetworkString> left_enemyCruiserName = new NetworkVariable<NetworkString>();
        public NetworkVariable<long> left_totalDestroyed1 = new NetworkVariable<long>();
        public NetworkVariable<long> left_totalDestroyed2 = new NetworkVariable<long>();
        public NetworkVariable<long> left_totalDestroyed3 = new NetworkVariable<long>();
        public NetworkVariable<long> left_totalDestroyed4 = new NetworkVariable<long>();
        public NetworkVariable<long> left_destructionScore = new NetworkVariable<long> { };

        public NetworkVariable<float> right_levelTimeInSeconds = new NetworkVariable<float>();
        public NetworkVariable<long> right_aircraftVal = new NetworkVariable<long>();
        public NetworkVariable<long> right_shipsVal = new NetworkVariable<long>();
        public NetworkVariable<long> right_cruiserVal = new NetworkVariable<long>();
        public NetworkVariable<long> right_buildingsVal = new NetworkVariable<long>();
        public NetworkVariable<NetworkString> right_enemyCruiserName = new NetworkVariable<NetworkString>();
        public NetworkVariable<long> right_totalDestroyed1 = new NetworkVariable<long>();
        public NetworkVariable<long> right_totalDestroyed2 = new NetworkVariable<long>();
        public NetworkVariable<long> right_totalDestroyed3 = new NetworkVariable<long>();
        public NetworkVariable<long> right_totalDestroyed4 = new NetworkVariable<long>();
        public NetworkVariable<long> right_destructionScore = new NetworkVariable<long> { };

        public Action OnNetworkSpawned;

        public Team GetTeam()
        {
            if (NetworkManager.Singleton.LocalClientId == playerAClientNetworkId.Value)
                return Team.LEFT;
            return Team.RIGHT;
        }

        public void CalculateScoresOfLeftPlayer()
        {
            Debug.Log("Calculate Left player score");
            left_levelTimeInSeconds.Value = PvPBattleSceneGodServer.deadBuildables_left[TargetType.PlayedTime].GetPlayedTime();
            left_aircraftVal.Value = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Aircraft].GetTotalDamageInCredits();
            left_shipsVal.Value = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Ships].GetTotalDamageInCredits();
            left_cruiserVal.Value = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Cruiser].GetTotalDamageInCredits();
            left_buildingsVal.Value = PvPBattleSceneGodServer.deadBuildables_left[TargetType.Buildings].GetTotalDamageInCredits();
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        left_totalDestroyed1.Value = PvPBattleSceneGodServer.deadBuildables_left[(TargetType)i].GetTotalDestroyed();
                        break;
                    case 1:
                        left_totalDestroyed2.Value = PvPBattleSceneGodServer.deadBuildables_left[(TargetType)i].GetTotalDestroyed();
                        break;
                    case 2:
                        left_totalDestroyed3.Value = PvPBattleSceneGodServer.deadBuildables_left[(TargetType)i].GetTotalDestroyed();
                        break;
                    case 3:
                        left_totalDestroyed4.Value = PvPBattleSceneGodServer.deadBuildables_left[(TargetType)i].GetTotalDestroyed();
                        break;
                }
            }

            Dictionary<TargetType, DeadBuildableCounter> deadBuildables = PvPBattleSceneGodServer.deadBuildables_left;
            long ds = 0;
            foreach (KeyValuePair<TargetType, DeadBuildableCounter> kvp in deadBuildables)
            {
                ds += kvp.Value.GetTotalDamageInCredits();
            }
            left_destructionScore.Value = ds;
        }

        public void CalculateScoresOfRightPlayer()
        {
            Debug.Log("Calculate Right player score");
            right_levelTimeInSeconds.Value = PvPBattleSceneGodServer.deadBuildables_right[TargetType.PlayedTime].GetPlayedTime();
            right_aircraftVal.Value = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Aircraft].GetTotalDamageInCredits();
            right_shipsVal.Value = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Ships].GetTotalDamageInCredits();
            right_cruiserVal.Value = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Cruiser].GetTotalDamageInCredits();
            right_buildingsVal.Value = PvPBattleSceneGodServer.deadBuildables_right[TargetType.Buildings].GetTotalDamageInCredits();
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        right_totalDestroyed1.Value = PvPBattleSceneGodServer.deadBuildables_right[(TargetType)i].GetTotalDestroyed();
                        break;
                    case 1:
                        right_totalDestroyed2.Value = PvPBattleSceneGodServer.deadBuildables_right[(TargetType)i].GetTotalDestroyed();
                        break;
                    case 2:
                        right_totalDestroyed3.Value = PvPBattleSceneGodServer.deadBuildables_right[(TargetType)i].GetTotalDestroyed();
                        break;
                    case 3:
                        right_totalDestroyed4.Value = PvPBattleSceneGodServer.deadBuildables_right[(TargetType)i].GetTotalDestroyed();
                        break;
                }
            }
            Dictionary<TargetType, DeadBuildableCounter> deadBuildables = PvPBattleSceneGodServer.deadBuildables_right;
            long ds = 0;
            foreach (KeyValuePair<TargetType, DeadBuildableCounter> kvp in deadBuildables)
            {
                ds += kvp.Value.GetTotalDamageInCredits();
            }
            right_destructionScore.Value = ds;
        }

        public static SynchedServerData Instance
        {
            get
            {
                if (sync_ServerData == null)
                {
                    sync_ServerData = FindObjectOfType<SynchedServerData>();
                }
                if (sync_ServerData == null)
                {
                    Debug.Log("No ServerSingleton in scene, did you run this from the bootstrap scene?");
                    return null;
                }
                return sync_ServerData;
            }
        }

        static SynchedServerData sync_ServerData;

        public void SetUserChosenTargetToServer(bool isAvailable, ulong objectID, Team team)
        {
            SetUserChosenTargetToServerRpc(isAvailable, objectID, team);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetUserChosenTargetToServerRpc(bool isAvailable, ulong objectID, Team team)
        {
            if (IsHost)
            {
                if (isAvailable)
                {
                    NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectID);
                    ITarget target = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<ITarget>();
                    if (target != null)
                    {
                        if (team == Team.LEFT)
                        {
                            PvPBattleSceneGodServer.Instance.playerACruiserUserChosenTargetManager.Target = target;
                        }
                        else
                        {
                            PvPBattleSceneGodServer.Instance.playerBCruiserUserChosenTargetManager.Target = target;
                        }
                    }
                    else
                    {
                        target = obj.gameObject.GetComponent<PvPCruiser>()?.Parse<ITarget>();
                        if (target != null)
                        {
                            if (team == Team.LEFT)
                            {
                                PvPBattleSceneGodServer.Instance.playerACruiserUserChosenTargetManager.Target = target;
                            }
                            else
                            {
                                PvPBattleSceneGodServer.Instance.playerBCruiserUserChosenTargetManager.Target = target;
                            }
                        }
                    }
                }
                else
                {
                    if (team == Team.LEFT)
                    {
                        PvPBattleSceneGodServer.Instance.playerACruiserUserChosenTargetManager.Target = null;
                    }
                    else
                    {
                        PvPBattleSceneGodServer.Instance.playerBCruiserUserChosenTargetManager.Target = null;
                    }
                }
            }
        }
        /// <summary>
        /// Clears the static singleton reference. MUST be called during FLEE cleanup to prevent stale references.
        /// </summary>
        public static void ClearInstance()
        {
            sync_ServerData = null;
            Debug.Log("PVP: SynchedServerData.ClearInstance - static singleton cleared");
        }
    }
}
