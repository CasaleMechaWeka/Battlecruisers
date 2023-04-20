using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Utils;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    public class ServerAuthoritativeLoadAllAsyncManager : NetworkBehaviour
    {
        [SerializeField]
        MultiplayBattleSceneGod m_MultiplayBattleSceneGodPrefab;
        // Start is called before the first frame update
        void Start()
        {
            DynamicPrefabLoadingUtilities.Init(NetworkManager.Singleton);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnDestroy()
        {
            DynamicPrefabLoadingUtilities.UnloadAndReleaseAllDynamicPrefabs();
            base.OnDestroy();
        }

        [ClientRpc]
        public void LoadMultiplayBattleSceneGodOnClientRpc()
        {
            var m_MultiplayBattleSceneGod = Instantiate(m_MultiplayBattleSceneGodPrefab);
        }



    }
}

