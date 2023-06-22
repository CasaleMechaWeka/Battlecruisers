
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public class PvPHealthBarController : PvPBaseProgressBarController, IPvPHealthBar
    {
        private IPvPDamagable _damagable;
        private float _maxHealth;
        private bool _followDamagable;

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

        public void Initialise(IPvPDamagable damagable /*,  bool followDamagable = false */)
        {
            Logging.Verbose(Tags.PROGRESS_BARS, damagable.ToString());

            Assert.IsNotNull(damagable);
        //  Assert.IsTrue(damagable.Health > 0);

            _damagable = damagable;
        //    _maxHealth = _damagable.Health;
            Offset = transform.position;
        //    _followDamagable = followDamagable;

        //    damagable.HealthChanged += Damagable_HealthChanged;
        }

        private void Damagable_HealthChanged(object sender, EventArgs e)
        {
            OnProgressChanged(_damagable.Health / _maxHealth);
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
            transform.position
                = new Vector3(
                    parentPosition.x + Offset.x,
                    parentPosition.y + Offset.y,
                    transform.position.z);
        }
    }
}

