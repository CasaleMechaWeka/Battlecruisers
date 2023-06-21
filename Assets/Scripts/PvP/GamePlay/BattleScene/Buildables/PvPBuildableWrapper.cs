

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
                gameObject.SetActive(newVal);
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
                Buildable.Initialise(PvPBattleSceneGodClient.Instance.uiManager);

                PvP_IsVisible.OnValueChanged += OnVisibleChanged;
            }

        }
    }
}

