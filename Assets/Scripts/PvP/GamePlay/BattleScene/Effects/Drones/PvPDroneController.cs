using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public class PvPDroneController : PvPPrefab, IPvPDroneController
    {
        public Animation _animation;

        public PvPFaction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;


        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            Assert.IsNotNull(_animation);
            //    gameObject.SetActive(false);
        }




        public void Activate(PvPDroneActivationArgs activationArgs)
        {
            gameObject.SetActive(true);
            SynchedServerData.Instance.ActivateNetworkObject(GetComponent<NetworkObject>().NetworkObjectId);
            gameObject.transform.position = activationArgs.Position;
            // clientRpc
            OnChangedPositionClientRpc(activationArgs.Position);
            Faction = activationArgs.Faction;

            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = PvPRandomGenerator.Instance.Value;
            _animation.Play();

            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(PvPDroneActivationArgs activationArgs, PvPFaction faction)
        {
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            SynchedServerData.Instance.DeactivateNetworkObject(GetComponent<NetworkObject>().NetworkObjectId);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
            }

        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public override void OnNetworkDespawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
        }

        [ClientRpc]
        private void OnChangedPositionClientRpc(Vector3 pos)
        {
            gameObject.transform.position = pos;
        }

    }
}