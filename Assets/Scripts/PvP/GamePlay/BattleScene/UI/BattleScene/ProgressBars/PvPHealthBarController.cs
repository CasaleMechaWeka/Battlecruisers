
using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public class PvPHealthBarController : PvPBaseProgressBarController, IPvPHealthBar
    {
        private IPvPDamagable _damagable;
        private float _maxHealth;
        private bool _followDamagable;
        public Image variantIcon;
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

        protected override void Awake()
        {
            base.Awake();
            variantIcon.enabled = false;
        }
        public void Initialise(IPvPDamagable damagable, bool followDamagable = false)
        {
            Logging.Verbose(Tags.PROGRESS_BARS, damagable.ToString());

            Assert.IsNotNull(damagable);
            Assert.IsTrue(damagable.Health > 0);

            _damagable = damagable;
            _maxHealth = _damagable.Health;
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
                Damagable_HealthChangedClientRpc(hp);
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

            UpdateVariantImage();
        }

        private void UpdatePosition()
        {
            Vector3 parentPosition = _damagable.GameObject.transform.position;
            transform.position
                = new Vector3(
                    parentPosition.x + Offset.x,
                    parentPosition.y + Offset.y,
                    transform.position.z);
        }

        private void UpdateVariantImage()
        {
            if (_damagable.Health != _maxHealth)
                variantIcon.gameObject.SetActive(true);
            else
                variantIcon.gameObject.SetActive(false);
        }

        [ClientRpc]
        private void Damagable_HealthChangedClientRpc(float hp)
        {
            OnProgressChanged(hp);
        }
    }
}

