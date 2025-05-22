using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public abstract class PvPTarget : PvPPrefab, ITarget, ITargetProxy
    {
        protected IPvPHealthTracker _healthTracker;
        protected ITime _time;

        public float maxHealth;

        public float MaxHealth => maxHealth;
        public bool IsDestroyed => IsServer ? Health == 0 : pvp_Health.Value == 0;
        public Faction Faction { get; protected set; }
        public GameObject GameObject => gameObject;
        public abstract TargetType TargetType { get; }
        public virtual TargetValue TargetValue => TargetValue.Low;
        public virtual Vector2 Velocity => new Vector2(0, 0);
        public abstract Vector2 Size { get; }
        public virtual Vector2 DroneAreaSize => Size;

        public ITransform Transform { get; private set; }

        public Action clickedRepairButton { get; set; }

        private const float NotZero = 99999f;

        // network variables
        /*        public NetworkVariable<float> pvp_Health = new NetworkVariable<float> { Value = NotZero };
                public NetworkVariable<bool> pvp_Destroyed = new NetworkVariable<bool> { Value = false };*/

        public NetworkVariable<float> pvp_Health = new NetworkVariable<float>();
        public NetworkVariable<bool> pvp_Destroyed = new NetworkVariable<bool>();

        public Quaternion Rotation
        {
            get { return (this != null && transform != null) ? transform.rotation : Quaternion.identity; }
            set
            {
                transform.rotation = value;
                if (IsServer)
                    CallRpc_SetRotation(value);
            }
        }
        public virtual Vector2 DroneAreaPosition => Position;
        public Vector2 Position
        {
            get { return (this != null && transform != null) ? transform.position : Vector3.zero; }
            set
            {
                transform.position = value;
                if (IsServer)
                    CallRpc_SetPosition(value);
            }
        }

        public Vector2 HealthBarOffset
        {
            get; set;
        }
        // IMaskHighlightable
        protected virtual Vector2 MaskHighlightableSize => Size;

        // Seems to be an okay approximation (for cruisers at least). ------------ (OLD COMMENT BY FELIX, default was 3)
        // 3 was too much, changing it to much lower ----------------------------- (NEW COMMENT BY DEAN)
        // For buildables ranges from 0.75 (tesla coil) to 5 (broadsides)
        private const float DEFAULT_HEALTH_GAIN_PER_DRONE_S = 0.667f;

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
        public float Health => _healthTracker != null ? _healthTracker.Health : maxHealth; /*IsServer ? (_healthTracker.Health >= 0f ? _healthTracker.Health : maxHealth) : (pvp_Health.Value >= 0 ? pvp_Health.Value : maxHealth);*/
        public RepairCommand RepairCommand { get; private set; }
        public float HealthGainPerDroneS { get; protected set; }

        private List<TargetType> _attackCapabilities;
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; private set; }
        public ITarget LastDamagedSource { get; private set; }
        ITarget ITargetProxy.Target => this;


        protected virtual void CallRpc_SetPosition(Vector3 pos)
        {

        }

        protected virtual void CallRpc_SetRotation(Quaternion rotation)
        {

        }

        protected virtual void CallRpc_ProgressControllerVisible(bool isEnabled)
        {

        }

        protected void AddAttackCapability(TargetType attackCapability)
        {
            if (!_attackCapabilities.Contains(attackCapability))
            {
                _attackCapabilities.Add(attackCapability);
            }
        }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _healthTracker = new PvPHealthTracker(this, maxHealth);
            _healthTracker.HealthGone += _health_HealthGone;

            _time = TimeBC.Instance;
            _attackCapabilities = new List<TargetType>();
            AttackCapabilities = new ReadOnlyCollection<TargetType>(_attackCapabilities);
            RepairCommand = new RepairCommand(RepairCommandExecute, CanRepairCommandExecute, this);
            HealthGainPerDroneS = DEFAULT_HEALTH_GAIN_PER_DRONE_S;

            Transform = new TransformBC(transform);
            clickedRepairButton += OnClickedRepairButton;
        }

        private void OnClickedRepairButton()
        {
            CallRpc_ClickedRepairButton();
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
            clickedRepairButton -= OnClickedRepairButton;
            DestroyMe();
        }

        protected virtual void DestroyMe()
        {
            if (!IsDestroyed)
            {
                _healthTracker.RemoveHealth(_healthTracker.MaxHealth);
            }
        }

        protected virtual void InternalDestroy()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroyed() { }

        protected void InvokeDestroyedEvent()
        {
            // Logging.Log(Tags.TARGET, $"{this} destroyed :/");
            pvp_Destroyed.Value = true;
            Destroyed?.Invoke(this, new DestroyedEventArgs(this));
            OnDestroyedEvent();
            CallRpc_ProgressControllerVisible(false);
        }

        protected virtual void OnDestroyedEvent()
        {
            if (IsClient)
                Destroyed?.Invoke(this, new DestroyedEventArgs(this));
        }

        public void TakeDamage(float damageAmount, ITarget damageSource, bool ignoreImmuneStatus = false)
        {
            if (IsBuildingImmune())
            {
                return;
            }
            // Logging.Log(Tags.TARGET, $"{this}  damageAmount: {damageAmount}  damageSource: {damageSource}");

            LastDamagedSource = damageSource;
            bool wasFullHealth = IsFullHealth;

            if (_healthTracker != null && _healthTracker.RemoveHealth(damageAmount))
            {
                OnTakeDamage();

                Damaged?.Invoke(this, new DamagedEventArgs(damageSource));
                try
                {
                    ulong objectId = ulong.MaxValue;
                    if (damageSource.GameObject.GetComponent<PvPBuilding>() != null)
                        objectId = (ulong)(damageSource.GameObject.GetComponent<PvPBuilding>()?._parent?.GetComponent<NetworkObject>()?.NetworkObjectId);
                    if (damageSource.GameObject.GetComponent<PvPUnit>() != null)
                        objectId = (ulong)(damageSource.GameObject.GetComponent<PvPUnit>()?._parent?.GetComponent<NetworkObject>()?.NetworkObjectId);
                    OnDamagedEventCalled(objectId);
                }
                catch (Exception ex)
                {
                    //    Debug.Log("Cruiser maybe not have _parent " + damageSource.GameObject.name);
                    Debug.LogError("Cruiser maybe not have _parent: " + ex.Message);
                }

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
            if (IsServer)
                return Health < maxHealth;
            return pvp_Health.Value < maxHealth;
        }

        public HighlightArgs CreateHighlightArgs(HighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Position, MaskHighlightableSize);
        }

        protected virtual List<SpriteRenderer> GetInGameRenderers()
        {
            return new List<SpriteRenderer>();
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {gameObject.GetInstanceID()}";
        }

        public void SetHealthToMax()
        {
            _healthTracker.SetHealth(maxHealth);
        }

        public virtual bool IsShield()
        {
            return false;
        }

        public virtual void SetBuildingImmunity(bool boo)
        {

        }

        public virtual bool IsBuildingImmune()
        {
            return false;
        }

        protected virtual void CallRpc_ClickedRepairButton()
        {

        }

        protected virtual void OnDamagedEventCalled(ulong objectId)
        {
            if (IsClient)
            {
                NetworkObject obj = PvPBattleSceneGodClient.Instance.GetNetworkObject(objectId);
                ITarget damageSource = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<ITarget>();
                if (damageSource == null)
                {
                    damageSource = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPUnit>>()?.Buildable?.Parse<IPvPUnit>();
                }
                if (damageSource != null)
                    Damaged?.Invoke(this, new DamagedEventArgs(damageSource));
            }

        }
    }
}

