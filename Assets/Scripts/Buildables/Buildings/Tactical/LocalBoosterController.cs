using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class LocalBoosterController : TacticalBuilding
    {
        private IBoostProvider _boostProvider;
        private ParticleSystem _boosterGlow;

        public override bool IsBoostable => true;

        public float boostMultiplier;

        public override void Initialise(UIManager uiManager)
        {
            base.Initialise(uiManager);

            _boostProvider = new BoostProvider(boostMultiplier);

            _boosterGlow = transform.FindNamedComponent<ParticleSystem>("LocalBoosterMasterGlow");
            _boosterGlow.gameObject.SetActive(false);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Logging.Log(Tags.LOCAL_BOOSTER, $"About to boost {_parentSlot.NeighbouringSlots.Count} slots :D");

            foreach (Slot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Add(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(true);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            foreach (Slot slot in _parentSlot.NeighbouringSlots)
            {
                slot.BoostProviders.Remove(_boostProvider);
            }

            _boosterGlow.gameObject.SetActive(false);
        }
    }
}
