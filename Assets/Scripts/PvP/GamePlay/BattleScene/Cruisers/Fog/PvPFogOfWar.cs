using BattleCruisers.Utils;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog
{
    public enum PvPFogStrength
    {
        Weak, Strong
    }

    public class PvPFogOfWar : PvPMonoBehaviourWrapper
    {
        public GameObject weakFog, strongFog;
        private PvPFogStrength currentStrength;

        public void Initialise(PvPFogStrength fogStrength)
        {
            Helper.AssertIsNotNull(weakFog, strongFog);
            IsVisible = false;
            currentStrength = fogStrength;
        }
        private void Start()
        {
            SetFogs();
        }
        private void SetFogs()
        {
            if (weakFog == null)
                weakFog = GameObject.Find("FrogOfWar-FRIENDLY");
            if (strongFog == null)
                strongFog = GameObject.Find("FrogOfWar-ENEMY");
        }
        protected override void CallRpc_SetPosition(Vector3 position)
        {
            SetPositionClientRpc(position);
        }

        protected override void CallRpc_SetVisible(bool isVisible)
        {
            SetVisibleClientRpc(isVisible);
        }

        [ClientRpc]
        private void SetPositionClientRpc(Vector3 position)
        {
            if (!IsHost)
                Position = position;
        }

        [ClientRpc]
        private void SetVisibleClientRpc(bool isVisible)
        {
            if (!IsHost)
            {
                IsVisible = isVisible;
            }
        }

        protected override void SetVisible(bool isVisible)
        {
            SetFogs();
            if (!isVisible)
            {
                weakFog?.SetActive(false);
                strongFog?.SetActive(false);
            }
            else
            {
                if (IsHost && IsOwner)
                {
                    weakFog?.SetActive(true);
                    strongFog?.SetActive(false);
                }
                if (IsHost && !IsOwner)
                {
                    weakFog?.SetActive(false);
                    strongFog?.SetActive(true);
                }
                if (!IsHost && !IsOwner)
                {
                    weakFog?.SetActive(false);
                    strongFog?.SetActive(true);
                }
                if (!IsHost && IsOwner)
                {
                    weakFog?.SetActive(true);
                    strongFog?.SetActive(false);
                }
            }
        }
    }
}