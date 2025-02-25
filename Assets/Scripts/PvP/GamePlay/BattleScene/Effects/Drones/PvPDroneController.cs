using System;
using BattleCruisers.Buildables;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public class PvPDroneController : PvPPrefab, IDroneController
    {
        public Animation _animation;
        public ParticleSystem _animatedDrone;
        public ParticleSystem _droneSpark;

        public Faction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public GameObject drone;
        private NetworkVariable<bool> pvp_IsDroneVisible = new NetworkVariable<bool>();
        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);
            Assert.IsNotNull(_animation);
        }
        public void Activate(DroneActivationArgs activationArgs)
        {
            pvp_IsDroneVisible.Value = true;
            gameObject.transform.position = activationArgs.Position;
            GetComponent<NetworkTransform>().Teleport(activationArgs.Position, transform.rotation, transform.localScale);
            OnSetFactionClientRpc(activationArgs.Faction);

            drone.SetActive(true);
            Faction = activationArgs.Faction;
            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = RandomGenerator.Instance.Value;
            _animation.Play();
            _animatedDrone.Play();
            _droneSpark.Play();
            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(DroneActivationArgs activationArgs, Faction faction)
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
                    state.normalizedTime = RandomGenerator.Instance.Value;
                    _animation.Play();
                    _animatedDrone.Play();
                    _droneSpark.Play();
                }
            }
        }

        [ClientRpc]
        private void OnSetFactionClientRpc(Faction faction)
        {
            if (!IsHost)
                Faction = faction;
        }
    }
}