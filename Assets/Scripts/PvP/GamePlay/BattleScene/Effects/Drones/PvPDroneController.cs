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
        public ParticleSystem _animatedDrone;
        public ParticleSystem _droneSpark;

        public PvPFaction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public GameObject drone;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            Assert.IsNotNull(_animation);
            //    gameObject.SetActive(false);
        }
        public void Activate(PvPDroneActivationArgs activationArgs)
        {
            drone.SetActive(true);
            OnVisibleDroneClientRpc(true);
            // SynchedServerData.Instance.ActivateNetworkObject(GetComponent<NetworkObject>().NetworkObjectId);
            gameObject.transform.position = activationArgs.Position;
            GetComponent<NetworkTransform>().Teleport(activationArgs.Position, transform.rotation, transform.localScale);
            // clientRpc
            //    OnChangedPositionClientRpc(activationArgs.Position);
            Faction = activationArgs.Faction;
            OnSetFactionClientRpc(activationArgs.Faction);
            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = PvPRandomGenerator.Instance.Value;
            _animation.Play();

            _animatedDrone.Play();
            _droneSpark.Play();
            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(PvPDroneActivationArgs activationArgs, PvPFaction faction)
        {
        }

        private void Start()
        {
            drone.SetActive(false);
        }
        public void Deactivate()
        {
            drone.SetActive(false);
            OnVisibleDroneClientRpc(false);
            //    SynchedServerData.Instance.DeactivateNetworkObject(GetComponent<NetworkObject>().NetworkObjectId);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
            }
        }
        public override void OnNetworkDespawn()
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
        }

        [ClientRpc]
        private void OnChangedPositionClientRpc(Vector3 pos)
        {
            if (!IsHost)
                gameObject.transform.position = pos;
        }

        [ClientRpc]
        private void OnVisibleDroneClientRpc(bool isVisible)
        {
            if (!IsHost)
            {
                drone.SetActive(isVisible);
                if(isVisible)
                {
                    AnimationState state = _animation["BuilderDrone"];
                    Assert.IsNotNull(state);
                    state.normalizedTime = PvPRandomGenerator.Instance.Value;
                    _animation.Play();
                    _animatedDrone.Play();
                    _droneSpark.Play();
                }
            }                
        }
        [ClientRpc]
        private void OnSetFactionClientRpc(PvPFaction faction)
        {
            if (!IsHost)
                Faction = faction;
        }
    }
}