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
        public Animation _animation;

        public PvPFaction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public GameObject _drone;

        public bool IsEnabled
        {
            get
            {
                return _drone.activeSelf;
            }
            set
            {     
                _drone.SetActive(value);

            }
        }

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            Assert.IsNotNull(_animation);
            Assert.IsNotNull(_drone);
            IsEnabled = false;
        }




        public void Activate(PvPDroneActivationArgs activationArgs)
        {
            IsEnabled = true;
            // clientRpc
         //   OnChangedEnabledValueClientRpc(IsEnabled);
            gameObject.transform.position = activationArgs.Position;
            // clientRpc
        //    OnChangedPositionClientRpc(activationArgs.Position);
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
            IsEnabled = false;
            // clientRpc
         //   OnChangedEnabledValueClientRpc(IsEnabled);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        [ClientRpc]
        private void OnChangedEnabledValueClientRpc(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        [ClientRpc]
        private void OnChangedPositionClientRpc(Vector3 pos)
        {
            gameObject.transform.position = pos;
        }

    }
}