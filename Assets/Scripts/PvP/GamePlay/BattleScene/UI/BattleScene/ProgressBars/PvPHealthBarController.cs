using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public class PvPHealthBarController : PvPBaseProgressBarController, IHealthBar
    {
        private IPvPDamagable _damagable;
        private float _maxHealth;
        private bool _followDamagable;

        [SerializeField]
        private bool manualOffsetOverride = false; // Checkbox to enable manual override

        [SerializeField]
        private Vector2 manualOffset = Vector2.zero; // Manual offset values

        private Vector2 _offset;
        public Action OffsetChanged;
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                UpdatePosition();
                OffsetChanged?.Invoke();
            }
        }
        private NetworkVariable<float> pvp_hp = new NetworkVariable<float>();

        protected override void Awake()
        {
            base.Awake();
            if (variantIcon != null) // shield generator
                variantIcon.enabled = false;
            pvp_hp.OnValueChanged += OnHpValueChanged;
        }

        private void OnHpValueChanged(float oldVal, float newVal)
        {
            if (!IsHost)
            {
                OnProgressChanged(newVal);
            }
        }

        public void Initialise(IPvPDamagable damagable, bool followDamagable = false)
        {
            Logging.Verbose(Tags.PROGRESS_BARS, damagable.ToString());

            Assert.IsNotNull(damagable);
            Assert.IsTrue(damagable.Health > 0);

            _damagable = damagable;
            _maxHealth = _damagable.MaxHealth;
            Offset = transform.position;
            _followDamagable = followDamagable;

            damagable.HealthChanged += Damagable_HealthChanged;
        }


        private void Damagable_HealthChanged(object sender, EventArgs e)
        {
            if (IsServer)
            {
                float hp = _damagable.Health / _maxHealth;
                OnProgressChanged(hp);
                pvp_hp.Value = hp;
            }
        }

        public void OverrideHealth(IPvPDamagable damagable)
        {
            Assert.IsNotNull(damagable);
            Assert.IsTrue(damagable.Health > 0);
            _maxHealth = _damagable.Health;
        }
        void LateUpdate()
        {
            if (_followDamagable)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            Vector3 parentPosition = _damagable.GameObject.transform.position;
            Vector2 offsetToUse = manualOffsetOverride ? manualOffset : Offset;

            if (!IsHost)
            {
                offsetToUse.x = -offsetToUse.x;
            }

            Vector3 newPosition = new Vector3(
                parentPosition.x + offsetToUse.x,
                parentPosition.y + offsetToUse.y,
                transform.position.z);

            transform.position = newPosition;
        }
    }
}

