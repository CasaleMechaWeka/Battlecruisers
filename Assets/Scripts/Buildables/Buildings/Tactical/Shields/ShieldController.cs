using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldController : Target
	{
        private IShieldStats _stats;
		private Ring _ring;
		private float _timeSinceDamageInS;

		public LineRenderer lineRenderer;
		public CircleCollider2D circleCollider;
		public HealthBarController healthBar;

		private const int NUM_OF_POINTS_IN_RING = 100;
		private const float HEALTH_BAR_Y_POSITION_MULTIPLIER = 1.2f;
		private const float SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER = 1.6f;
		private const float HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER = 0.025f;

		public override TargetType TargetType { get { return TargetType.Buildings; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _stats = GetComponent<IShieldStats>();
            Assert.IsNotNull(_stats);
        }

		public void Initialise(Faction faction)
		{
			Faction = faction;

            _ring = new Ring(_stats.ShieldRadiusInM, NUM_OF_POINTS_IN_RING, lineRenderer);
			_timeSinceDamageInS = 0;
			circleCollider.radius = _stats.ShieldRadiusInM;

			SetupHealthBar();
		}

		private void SetupHealthBar()
		{
			healthBar.Initialise(this);
			
			float yPos = HEALTH_BAR_Y_POSITION_MULTIPLIER * _stats.ShieldRadiusInM;
            healthBar.UpdateOffset(new Vector2(0, yPos));
			
			float width = SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER * _stats.ShieldRadiusInM;
			float height = HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER * width;
			healthBar.UpdateSize(width, height);
		}

		void Update()
		{
			// Eat into recharge delay
			if (Health < maxHealth)
			{
				_timeSinceDamageInS += Time.deltaTime;

				// Heal
				if (_timeSinceDamageInS >= _stats.ShieldRechargeDelayInS)
				{
					if (IsDestroyed)
					{
						EnableShield();
					}

					RepairCommandExecute(_stats.ShieldRechargeRatePerS * Time.deltaTime);
				}
			}
		}

		protected override void OnHealthGone()
		{
			DisableShield();
			InvokeDestroyedEvent();
		}

		protected override void OnTakeDamage()
		{
			_timeSinceDamageInS = 0;
		}

		private void EnableShield()
		{
			_ring.Enabled = true;
			circleCollider.enabled = true;
		}

		private void DisableShield()
		{
			_ring.Enabled = false;
			circleCollider.enabled = false;
		}
	}
}
