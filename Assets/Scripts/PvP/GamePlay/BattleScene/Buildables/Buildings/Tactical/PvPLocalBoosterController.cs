using BattleCruisers.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Utils;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPLocalBoosterController : PvPTacticalBuilding
    {
        private IBoostProvider _boostProvider;
        private ParticleSystem _boosterGlow;
        public override bool IsBoostable => true;

        public float boostMultiplier;

        public override void Initialise()
        {
            base.Initialise();

            _boostProvider = new BoostProvider(boostMultiplier);

            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                // Logging.Log(Tags.LOCAL_BOOSTER, $"About to boost {_parentSlot.NeighbouringSlots.Count} slots :D");

                foreach (PvPSlot slot in _parentSlot.NeighbouringSlots)
                {
                    slot.BoostProviders.Add(_boostProvider);
                }

                _boosterGlow.gameObject.SetActive(true);

                OnEnableBoosterGlowClientRpc(true);
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }

        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (PvPSlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(false);
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        [ClientRpc]
        private void OnEnableBoosterGlowClientRpc(bool enabled)
        {
            if (!IsHost)
                _boosterGlow.gameObject.SetActive(enabled);
        }
    }
}
