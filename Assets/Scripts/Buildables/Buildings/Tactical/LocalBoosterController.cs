using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class LocalBoosterController : TacticalBuilding
    {
        private IBoostProvider _boostProvider;
        private ParticleSystem _boosterGlow;

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Booster;
        public override bool IsBoostable => true;

        public float boostMultiplier;

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            _boostProvider = _factoryProvider.BoostFactory.CreateBoostProvider(boostMultiplier);

            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Logging.Log(Tags.LOCAL_BOOSTER, $"About to boost {_parentSlot.NeighbouringSlots.Count} slots :D");

            foreach (ISlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Add(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(true);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (ISlot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(false);
        }
    }
}
