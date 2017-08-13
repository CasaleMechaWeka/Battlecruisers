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
		private Renderer _turretBarrelRenderer;
		protected IBarrelWrapper _barrelWrapper;

		protected override Renderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = _turretBase.GetComponent<Renderer>();
				}
				return _renderer;
			}
		}

		public override Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = _buildableProgress.FillableImageSprite;
				}
				return _sprite;
			}
		}

		public override float Damage { get { return _barrelWrapper.TurretStats.DamagePerS; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_turretBase = transform.Find("Base").gameObject;
			_turretBaseRenderer = _turretBase.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBaseRenderer);
			
            GameObject turretBarrel = transform.Find("BarrelWrapper/Barrel").gameObject;
			_turretBarrelRenderer = turretBarrel.GetComponent<Renderer>();
			Assert.IsNotNull(_turretBarrelRenderer);
			
            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _barrelWrapper.Initialise(_factoryProvider, enemyFaction, AttackCapabilities);
		}

		protected override void EnableRenderers(bool enabled)
		{
			_turretBaseRenderer.enabled = enabled;
			_turretBarrelRenderer.enabled = enabled;
		}
	}
}
