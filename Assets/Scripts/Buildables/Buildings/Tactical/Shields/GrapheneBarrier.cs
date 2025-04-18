using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class GrapheneBarrier : TacticalBuilding
    {

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Shields;
        public override TargetValue TargetValue => TargetValue.Low;
        public override bool IsBoostable => false;
        private Animator animator;

        public GrapheneSectorShieldController _shieldController;

        protected override void AddBuildRateBoostProviders(
            GlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders);
        }

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _shieldController = GetComponentInChildren<GrapheneSectorShieldController>(includeInactive: true);
            Assert.IsNotNull(_shieldController);
            _shieldController.StaticInitialise();

            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator component could not be found.");
            animator.enabled = false;
            Assert.IsNotNull(_shieldController, "Shield controller could not be found.");
            _shieldController.onShieldDepleted.AddListener(OnShieldDepleted);
            _shieldController.onShieldDamaged.AddListener(OnShieldDamaged);

            HealthChanged += OnHealthChanged;
        }

        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _shieldController.Initialise(Faction);
            _shieldController.gameObject.SetActive(false);

            _localBoosterBoostableGroup.AddBoostable(_shieldController.Stats);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            //start deploy animation
            animator.enabled = true;

            _shieldController.gameObject.SetActive(true);
            _shieldController.ApplyVariantStats(this);
        }

        private void OnShieldDepleted()
        {
            base.Destroy();
        }

        private void OnShieldDamaged()
        {
            float newHealth = maxHealth / _shieldController.maxHealth * _shieldController.Health;

            TakeDamage(Health - newHealth, EnemyCruiser, true);
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            Debug.Log("OnHealthChanged");
            _shieldController.SetShieldHealth(_shieldController.maxHealth / maxHealth * Health);
        }
    }
}
