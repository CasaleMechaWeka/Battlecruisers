using BattleCruisers.Buildables.Pools;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using UnityEngine;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class GrapheneBarrier : SectorShieldGenerator
    {

        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Shields;
        public override TargetValue TargetValue => TargetValue.Low;
        public override bool IsBoostable => false;
        private Animator animator;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);
            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator, "Animator component could not be found.");
            animator.enabled = false;
            Assert.IsNotNull(_shieldController, "Shield controller could not be found.");
            _shieldController.onShieldDepleted.AddListener(OnShieldDepleted);
        }

        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            //start deploy animation
            animator.enabled = true;
        }

        private void OnShieldDepleted()
        {
            base.Destroy();
        }

    }
}
