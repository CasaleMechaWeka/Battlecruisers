using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldController : Target
	{
        private ISoundPlayer _soundPlayer;
        private float _timeSinceDamageInS;

        public GameObject visuals;
        public CircleCollider2D circleCollider;
        public HealthBarController healthBar;

        private const int NUM_OF_POINTS_IN_RING = 100;
        private const float HEALTH_BAR_Y_POSITION_MULTIPLIER = 1.2f;
        private const float SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER = 1.6f;
        private const float HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER = 0.025f;

        public IShieldStats Stats { get; private set; } 
		public override TargetType TargetType => TargetType.Buildings;

        private Vector2 _size;
        public override Vector2 Size => _size;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Helper.AssertIsNotNull(visuals, circleCollider, healthBar);

            Stats = GetComponent<IShieldStats>();
            Assert.IsNotNull(Stats);

            float diameter = 2 * Stats.ShieldRadiusInM;
            _size = new Vector2(diameter, diameter); 
        }

		public void Initialise(Faction faction, ISoundPlayer soundPlayer)
		{
			Faction = faction;

            _soundPlayer = soundPlayer;
			_timeSinceDamageInS = 0;
			circleCollider.radius = Stats.ShieldRadiusInM;

			SetupHealthBar();
		}

		private void SetupHealthBar()
		{
			healthBar.Initialise(this);
			
			float yPos = HEALTH_BAR_Y_POSITION_MULTIPLIER * Stats.ShieldRadiusInM;
            healthBar.Offset = new Vector2(0, yPos);
			
			float width = SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER * Stats.ShieldRadiusInM;
			float height = HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER * width;
			healthBar.UpdateSize(width, height);
		}

        // PERF:  Don't need to do this every frame
		void Update()
		{
			// Eat into recharge delay
			if (Health < maxHealth)
			{
				_timeSinceDamageInS += _time.DeltaTime;

				// Heal
				if (_timeSinceDamageInS >= Stats.ShieldRechargeDelayInS)
				{
					if (IsDestroyed)
					{
						EnableShield();
					}

					RepairCommandExecute(Stats.ShieldRechargeRatePerS * _time.DeltaTime);

                    if (Health == maxHealth)
                    {
                        _soundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyCharged, Position);
                    }
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
            _soundPlayer.PlaySoundAsync(SoundKeys.Shields.HitWhileActive, Position);
        }

        private void EnableShield()
		{
            visuals.SetActive(true);
			circleCollider.enabled = true;
        }

        private void DisableShield()
		{
            visuals.SetActive(false);
            circleCollider.enabled = false;
            _soundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyDepleted, Position);
		}
	}
}
