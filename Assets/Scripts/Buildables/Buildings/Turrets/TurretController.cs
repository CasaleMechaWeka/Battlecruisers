using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class TurretController : Building
	{
		private GameObject _turretBase;
		private Renderer _turretBaseRenderer;
		protected IBarrelWrapper _barrelWrapper;

		public List<TargetType> attackCapabilities;

        protected override Renderer Renderer { get { return _turretBaseRenderer; } }
        public override Sprite Sprite { get { return _buildableProgress.FillableImageSprite; } }
		public override float Damage { get { return _barrelWrapper.TurretStats.DamagePerS; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_turretBase = transform.Find("Base").gameObject;
			_turretBaseRenderer = _turretBase.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBaseRenderer);
			
            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();

            Assert.IsNotNull(attackCapabilities);
            Assert.IsTrue(attackCapabilities.Count != 0);
            _attackCapabilities.AddRange(attackCapabilities);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			_barrelWrapper.Initialise(_factoryProvider, enemyFaction, AttackCapabilities);

            _boostableGroup.AddBoostable(_barrelWrapper.TurretStats);
		}

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _barrelWrapper.StartAttackingTargets();
        }

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;

            foreach (Renderer renderer in _barrelWrapper.Renderers)
            {
                renderer.enabled = enabled;
            }
		}
    }
}
