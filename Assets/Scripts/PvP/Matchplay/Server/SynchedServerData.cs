using System;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.Shared
{
    public class SynchedServerData : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<NetworkString> serverID = new NetworkVariable<NetworkString>();
        public NetworkVariable<Map> map = new NetworkVariable<Map>();
        public NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>();
        public NetworkVariable<GameQueue> gameQueue = new NetworkVariable<GameQueue>();

        public NetworkVariable<NetworkString> playerAPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<ulong> playerAClientNetworkId = new NetworkVariable<ulong>();
        public NetworkVariable<NetworkString> playerAName = new NetworkVariable<NetworkString>();
        public NetworkVariable<long> playerAScore = new NetworkVariable<long>();
        public NetworkVariable<NetworkString> captainAPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<float> playerARating = new NetworkVariable<float>();

        public NetworkVariable<NetworkString> playerBPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<ulong> playerBClientNetworkId = new NetworkVariable<ulong>();
        public NetworkVariable<NetworkString> playerBName = new NetworkVariable<NetworkString>();
        public NetworkVariable<long> playerBScore = new NetworkVariable<long>();
        public NetworkVariable<NetworkString> captainBPrefabName = new NetworkVariable<NetworkString>();
        public NetworkVariable<float> playerBRating = new NetworkVariable<float>();

        public NetworkVariable<bool> IsServerInitialized = new NetworkVariable<bool>();

        public Action OnNetworkSpawned;

        public Team GetTeam()
        {
            if (NetworkManager.Singleton.LocalClientId == playerAClientNetworkId.Value)
                return Team.LEFT;
            return Team.RIGHT;
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

        public void SetUserChosenTargetToServer(bool isAvailable ,ulong objectID, Team team)
        {            
            SetUserChosenTargetToServerRpc(isAvailable, objectID, team);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetUserChosenTargetToServerRpc(bool isAvailable, ulong objectID, Team team)
        {
            if(IsHost)
            {
                if(isAvailable)
                {
                    NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectID);
                    IPvPTarget target = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<IPvPTarget>();
                    if(target != null)
                    {
                        if(team == Team.LEFT)
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
                        target = obj.gameObject.GetComponent<PvPCruiser>()?.Parse<IPvPTarget>();
                        if(target != null)
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
    }
}
