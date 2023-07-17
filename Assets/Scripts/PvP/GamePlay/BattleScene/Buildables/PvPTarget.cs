using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public abstract class PvPTarget : PvPPrefab, IPvPTarget, IPvPTargetProxy
    {
        protected IPvPHealthTracker _healthTracker;
        protected IPvPTime _time;

        public float maxHealth;

        public float MaxHealth => maxHealth;
        public bool IsDestroyed => IsServer ? Health == 0 : pvp_Health.Value == 0;
        public PvPFaction Faction { get; protected set; }
        public GameObject GameObject => gameObject;
        public abstract PvPTargetType TargetType { get; }
        public virtual PvPTargetValue TargetValue => PvPTargetValue.Low;
        public virtual Vector2 Velocity => new Vector2(0, 0);
        public abstract Vector2 Size { get; }
        public virtual Vector2 DroneAreaSize => Size;

        public IPvPTransform Transform { get; private set; }

        public Action clickedRepairButton { get; set; }

        private const float NotZero = 100.0f;

        // network variables
        public NetworkVariable<float> pvp_Health = new NetworkVariable<float> { Value = NotZero };
        public NetworkVariable<bool> pvp_Destroyed = new NetworkVariable<bool> { Value = false };

        public Quaternion Rotation
        {
            get { return transform.rotation; }
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
            get { return transform.position; }
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

        public event EventHandler<PvPDestroyedEventArgs> Destroyed;
        public event EventHandler<PvPDamagedEventArgs> Damaged;

        public event EventHandler HealthChanged
        {
            add { _healthTracker.HealthChanged += value; }
            remove { _healthTracker.HealthChanged -= value; }
        }

        private bool IsFullHealth => Health == maxHealth;
        public virtual Color Color { set { /* empty */ } }
        public bool IsInScene => gameObject.scene.IsValid();
        public float Health => _healthTracker.Health; /*IsServer ? (_healthTracker.Health >= 0f ? _healthTracker.Health : maxHealth) : (pvp_Health.Value >= 0 ? pvp_Health.Value : maxHealth);*/
        public IPvPRepairCommand RepairCommand { get; private set; }
        public float HealthGainPerDroneS { get; protected set; }

        private List<PvPTargetType> _attackCapabilities;
        public ReadOnlyCollection<PvPTargetType> AttackCapabilities { get; private set; }
        public IPvPTarget LastDamagedSource { get; private set; }
        IPvPTarget IPvPTargetProxy.Target => this;


        protected virtual void CallRpc_SetPosition(Vector3 pos)
        {

        }

        protected virtual void CallRpc_SetRotation(Quaternion rotation)
        {

        }

        protected virtual void CallRpc_ProgressControllerVisible(bool isEnabled)
        {

        }

        protected void AddAttackCapability(PvPTargetType attackCapability)
        {
            if (!_attackCapabilities.Contains(attackCapability))
            {
                _attackCapabilities.Add(attackCapability);
            }
        }

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            _healthTracker = new PvPHealthTracker(this, maxHealth);
            _healthTracker.HealthGone += _health_HealthGone;

            _time = PvPTimeBC.Instance;
            _attackCapabilities = new List<PvPTargetType>();
            AttackCapabilities = new ReadOnlyCollection<PvPTargetType>(_attackCapabilities);
            RepairCommand = new PvPRepairCommand(RepairCommandExecute, CanRepairCommandExecute, this);
            HealthGainPerDroneS = DEFAULT_HEALTH_GAIN_PER_DRONE_S;

            Transform = new PvPTransformBC(transform);
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
                /*             PvP_Rotation.OnValueChanged -= OnPvPRotationChanged;
                             PvP_Position.OnValueChanged -= OnPvPPositionChanged;*/
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
            Destroyed?.Invoke(this, new PvPDestroyedEventArgs(this));
            OnDestroyedEvent();
            CallRpc_ProgressControllerVisible(false);
        }

        protected virtual void OnDestroyedEvent()
        {
            if (IsClient)
                Destroyed?.Invoke(this, new PvPDestroyedEventArgs(this));
        }

        public void TakeDamage(float damageAmount, IPvPTarget damageSource)
        {
            if (IsBuildingImmune())
            {
                return;
            }
            // Logging.Log(Tags.TARGET, $"{this}  damageAmount: {damageAmount}  damageSource: {damageSource}");

            LastDamagedSource = damageSource;
            bool wasFullHealth = IsFullHealth;

            if (_healthTracker.RemoveHealth(damageAmount))
            {
                OnTakeDamage();

                Damaged?.Invoke(this, new PvPDamagedEventArgs(damageSource));
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
                    Debug.Log("Cruiser maybe not have _parent " + damageSource.GameObject.name);
                }

                if (wasFullHealth)
                {
                    RepairCommand.EmitCanExecuteChanged();
                }
            }
        }

        /*        private void LateUpdate()
                {
                    if (IsClient)
                    {

                        Position = PvP_Position.Value;
                        Rotation = PvP_Rotation.Value;
                        Debug.Log("aaa");
                    }
                }*/

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

        public PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory)
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
                NetworkObject[] objs = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
                foreach (NetworkObject obj in objs)
                {
                    if (obj.NetworkObjectId == objectId)
                    {
                        IPvPTarget damageSource = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPBuilding>>()?.Buildable?.Parse<IPvPTarget>();
                        if (damageSource == null)
                        {
                            damageSource = obj.gameObject.GetComponent<PvPBuildableWrapper<IPvPUnit>>()?.Buildable?.Parse<IPvPUnit>();
                        }
                        if (damageSource != null)
                            Damaged?.Invoke(this, new PvPDamagedEventArgs(damageSource));
                    }
                }
            }

        }
    }
}

