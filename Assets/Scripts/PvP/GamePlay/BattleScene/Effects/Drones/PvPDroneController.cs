using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public class PvPDroneController : PvPPrefab, IPvPDroneController
    {
        private Animation _animation;

        public PvPFaction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public NetworkVariable<bool> PvP_IsEnabled = new NetworkVariable<bool>();
        public NetworkVariable<Vector3> PvP_Position = new NetworkVariable<Vector3>();
        public bool IsEnabled
        {
            get {
                return gameObject.activeSelf;
            }
            set 
            {
                gameObject.SetActive(value);
                if (IsServer)
                {
                    PvP_IsEnabled.Value = value;
                }
            }
        }

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            _animation = GetComponentInChildren<Animation>();
            Assert.IsNotNull(_animation);

          //  gameObject.SetActive(false);
            IsEnabled = false;

        }

        public void Activate(PvPDroneActivationArgs activationArgs)
        {         

            gameObject.transform.position = activationArgs.Position;
            PvP_Position.Value = activationArgs.Position;
          //  gameObject.SetActive(true);
            IsEnabled = true;

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
         //   gameObject.SetActive(false);
            IsEnabled = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        private void Start()
        {
            if (IsClient)
            {
                PvP_IsEnabled.OnValueChanged += OnPvPIsEnabledChanged;
                PvP_Position.OnValueChanged += OnPvPPositionChanged;
            }

        }

        private void OnPvPIsEnabledChanged(bool oldVal, bool newVal)
        {
            IsEnabled = newVal;
        }

        private void OnPvPPositionChanged(Vector3 oldVal, Vector3 newVal)
        {
            gameObject.transform.position = newVal;
        }
/*        private void Update()
        {

            if (IsServer)
            {
                PvP_IsEnabled.Value = IsEnabled;                
            }
            if (IsClient)
            {
                IsEnabled = PvP_IsEnabled.Value;
            }
        }*/
    }
}