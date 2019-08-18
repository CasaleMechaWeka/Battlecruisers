using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public abstract class Target : MonoBehaviour, ITarget
    {
        protected IHealthTracker _healthTracker;
        protected IAudioSource _audioSource;
        protected ITime _time;

        public float maxHealth;

        public float MaxHealth => maxHealth;
        public bool IsDestroyed => Health == 0;
        public Faction Faction { get; protected set; }
        public GameObject GameObject => gameObject;
        public abstract TargetType TargetType { get; }
        public virtual TargetValue TargetValue => TargetValue.Low;
        public virtual Vector2 Velocity => new Vector2(0, 0);
        public abstract Vector2 Size { get; }
        public ITransform Transform { get; private set; }

        public Quaternion Rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        public Vector2 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        // IMaskHighlightable
        protected virtual Vector2 MaskHighlightableSize => Size;

        // Seems to be an okay approximation (for cruisers at least).
        // For buildables ranges from 0.75 (tesla coil) to 5 (broadsides)
		private const float DEFAULT_HEALTH_GAIN_PER_DRONE_S = 3;

        public event EventHandler<DestroyedEventArgs> Destroyed;
        public event EventHandler<DamagedEventArgs> Damaged;

        public event EventHandler HealthChanged
        {
            add { _healthTracker.HealthChanged += value; }
            remove { _healthTracker.HealthChanged -= value; }
        }

        private bool IsFullHealth => Health == maxHealth;
        public virtual Color Color { set { /* empty */ } }
        public bool IsInScene => gameObject.scene.IsValid();
        public float Health => _healthTracker.Health;
        public IRepairCommand RepairCommand { get; private set; }
        public float HealthGainPerDroneS { get; protected set; }

        private List<TargetType> _attackCapabilities;
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; private set; }
        protected void AddAttackCapability(TargetType attackCapability)
        {
            if (!_attackCapabilities.Contains(attackCapability))
            {
                _attackCapabilities.Add(attackCapability);
			}
        }

        // Lazily initialise so that the StaticInitialise() of all classes in
        // inheritance tree have completed.
        private ITextMesh _numOfRepairDronesText;
        public ITextMesh NumOfRepairDronesText
        { 
            get
            {
                if (_numOfRepairDronesText == null)
                {
                    _numOfRepairDronesText = GetRepairDroneNumText();
                }
                return _numOfRepairDronesText;
            }
        }

        public virtual void StaticInitialise()
		{
            _healthTracker = new HealthTracker(maxHealth);
            _healthTracker.HealthGone += _health_HealthGone;

            _time = TimeBC.Instance;
            _attackCapabilities = new List<TargetType>();
            AttackCapabilities = new ReadOnlyCollection<TargetType>(_attackCapabilities);
            RepairCommand = new RepairCommand(RepairCommandExecute, CanRepairCommandExecute, this);
            HealthGainPerDroneS = DEFAULT_HEALTH_GAIN_PER_DRONE_S;

            AudioSource audioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(audioSource);
            _audioSource = new AudioSourceBC(audioSource);

            Transform = new TransformBC(transform);
        }

        protected virtual ITextMesh GetRepairDroneNumText()
        {
            return new DummyTextMesh();
        }

        private void _health_HealthGone(object sender, EventArgs e)
        {
            OnHealthGone();
        }

		protected virtual void OnHealthGone()
		{
			OnDestroyed();
			InvokeDestroyedEvent();
			InternalDestroy();
        }

        public void Destroy()
        {
            Assert.IsFalse(IsDestroyed, "Same target should not be destroyed more than once scrub :P");
            _healthTracker.RemoveHealth(_healthTracker.MaxHealth);
		}

		protected virtual void InternalDestroy()
		{
            Destroy(gameObject);
        }

        protected virtual void OnDestroyed() { }

		protected void InvokeDestroyedEvent()
		{
            Logging.Log(Tags.TARGET, this + " destroyed :/");

			Destroyed?.Invoke(this, new DestroyedEventArgs(this));
		}

        public void TakeDamage(float damageAmount, ITarget damageSource)
		{
            bool wasFullHealth = IsFullHealth;

            if (_healthTracker.RemoveHealth(damageAmount))
            {
	            OnTakeDamage();

                Damaged?.Invoke(this, new DamagedEventArgs(damageSource));

                if (wasFullHealth)
                {
                    RepairCommand.EmitCanExecuteChanged();
                }
			}
		}

		protected virtual void OnTakeDamage() { }

		protected void RepairCommandExecute(float repairAmount)
		{
			Assert.IsTrue(CanRepairCommandExecute());

            if (_healthTracker.AddHealth(repairAmount))
            {
                if (IsFullHealth)
                {
                    RepairCommand.EmitCanExecuteChanged();
                }
            }
		}

        protected virtual bool CanRepairCommandExecute()
        {
            return Health < maxHealth;
        }

        public HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Position, MaskHighlightableSize);
        }

        protected virtual List<SpriteRenderer> GetInGameRenderers()
        {
            return new List<SpriteRenderer>();
        }
    }
}
