using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    // FELIX  Test :)
    public class HealthDial : IHealthDial
    {
        private readonly IFillableImage _healthDialImage;

        private IDamagable _damagable;
        public IDamagable Damagable
        {
            set
            {
                if (_damagable != null)
                {
                    _damagable.HealthChanged -= _damagable_HealthChanged;
                }

                _damagable = value;

                if (_damagable != null)
                {
                    UpdateDial();
                    _damagable.HealthChanged += _damagable_HealthChanged;
                }
            }
        }

        public HealthDial(IFillableImage healthDialImage)
        {
            Assert.IsNotNull(healthDialImage);
            _healthDialImage = healthDialImage;
        }

        private void _damagable_HealthChanged(object sender, EventArgs e)
        {
            UpdateDial();
        }

        private void UpdateDial()
        {
            float proportionOfMaxHealth = _damagable.Health / _damagable.MaxHealth;
            _healthDialImage.FillAmount = proportionOfMaxHealth;
        }
    }
}