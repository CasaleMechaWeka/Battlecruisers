using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
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
        private NetworkVariable<bool> pvp_IsDroneVisible = new NetworkVariable<bool>();
        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);
            Assert.IsNotNull(_animation);
        }
        public void Activate(PvPDroneActivationArgs activationArgs)
        {
            pvp_IsDroneVisible.Value = true;
            gameObject.transform.position = activationArgs.Position;
            GetComponent<NetworkTransform>().Teleport(activationArgs.Position, transform.rotation, transform.localScale);
            OnSetFactionClientRpc(activationArgs.Faction);

            drone.SetActive(true);
            Faction = activationArgs.Faction;
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
            pvp_IsDroneVisible.Value = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            drone.SetActive(pvp_IsDroneVisible.Value);
        }

        public override void OnNetworkSpawn()
        {
            pvp_IsDroneVisible.OnValueChanged += OnChangedDroneVisible;
            if (IsClient)
            {
                PvPBattleSceneGodClient.Instance.AddNetworkObject(GetComponent<NetworkObject>());
            }
            if (IsHost)
                pvp_IsDroneVisible.Value = false;
        }
        public override void OnNetworkDespawn()
        {
            pvp_IsDroneVisible.OnValueChanged -= OnChangedDroneVisible;
            if (IsClient)
            {
                PvPBattleSceneGodClient.Instance.RemoveNetworkObject(GetComponent<NetworkObject>());
            }
        }

        private void OnChangedDroneVisible(bool oldVal, bool newVal)
        {
            if (!IsHost)
            {
                drone.SetActive(newVal);
                if (newVal)
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