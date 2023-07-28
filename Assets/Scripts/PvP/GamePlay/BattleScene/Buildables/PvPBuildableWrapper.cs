

using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public class PvPBuildableWrapper<TPvPBuildable> : PvPPrefab, IPvPBuildableWrapper<TPvPBuildable> where TPvPBuildable : class, IPvPBuildable
    {
        public TPvPBuildable Buildable { get; private set; }

        public PvPBuildableWrapper<TPvPBuildable> UnityObject => this;
        public NetworkVariable<bool> PvP_IsVisible = new NetworkVariable<bool>();
        public bool IsVisible
        {
            get
            {
                return gameObject.activeSelf;
            }
            set
            {
                gameObject.SetActive(value);
                if (IsServer)
                    PvP_IsVisible.Value = value;
            }
        }

        private void OnVisibleChanged(bool oldVal, bool newVal)
        {
            if (IsClient)
                // gameObject.SetActive(newVal);
                IsVisible = newVal;
        }

        public override void StaticInitialise(ILocTable commonStrings)
        {
            Buildable = GetComponentInChildren<TPvPBuildable>();
            Assert.IsNotNull(Buildable);

            PvPHealthBarController healthBar = GetComponentInChildren<PvPHealthBarController>();
            Assert.IsNotNull(healthBar);

            Buildable.StaticInitialise(gameObject, healthBar, commonStrings);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                Buildable = GetComponentInChildren<TPvPBuildable>();
                Assert.IsNotNull(Buildable);

                PvPHealthBarController healthBar = GetComponentInChildren<PvPHealthBarController>();
                Assert.IsNotNull(healthBar);

                Buildable.StaticInitialise(gameObject, healthBar, PvPBattleSceneGodClient.Instance.commonStrings);
                Buildable.Initialise(PvPBattleSceneGodClient.Instance.factoryProvider, PvPBattleSceneGodClient.Instance.uiManager);

                PvP_IsVisible.OnValueChanged += OnVisibleChanged;

                if (IsOwner)
                {
                    Buildable.ParentCruiser = PvPBattleSceneGodClient.Instance.playerCruiser;
                }
            //    PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsClient)
            {
            //    PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
                PvP_IsVisible.OnValueChanged -= OnVisibleChanged;
            }

        }

        protected void FixedUpdate()
        {
            if (IsClient)
            {
                if (gameObject.activeSelf != PvP_IsVisible.Value)
                {
                    gameObject.SetActive(PvP_IsVisible.Value);  // not sure, but pvp_isvisible is not synched sometimes.
                }
            }
        }
    }
}

