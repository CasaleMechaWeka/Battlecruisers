using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPLocalBoosterController : PvPTacticalBuilding
    {
        private IPvPBoostProvider _boostProvider;
        private ParticleSystem _boosterGlow;

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.Booster;
        public override bool IsBoostable => true;

        public float boostMultiplier;

        public override void Initialise(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            _boostProvider = _factoryProvider.BoostFactory.CreateBoostProvider(boostMultiplier);

            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            // Logging.Log(Tags.LOCAL_BOOSTER, $"About to boost {_parentSlot.NeighbouringSlots.Count} slots :D");

            foreach (IPvPSlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Add(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(true);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (IPvPSlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(false);
        }
    }
}
