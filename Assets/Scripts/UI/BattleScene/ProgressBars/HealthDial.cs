using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class HealthDial<TDamagable> where TDamagable : IDamagable
    {
        private readonly Image _healthDialImage;
        private readonly IFilter<TDamagable> _visibilityFilter;
        private DamageTakenIndicator _damageTakenIndicator;

        private float previousHealthProportion;

        private TDamagable _damagable;
        public TDamagable Damagable
        {
            set
            {
                if (_damagable != null)
                {
                    _damagable.HealthChanged -= _damagable_HealthChanged;
                    _healthDialImage.gameObject.SetActive(false);
                }

                _damagable = value;

                if (_damagable != null
                    && _visibilityFilter.IsMatch(_damagable))
                {
                    _healthDialImage.gameObject.SetActive(true);
                    UpdateDial();
                    _damagable.HealthChanged += _damagable_HealthChanged;
                }
            }
        }

        public HealthDial(Image healthDialImage, IFilter<TDamagable> visibilityFilter, DamageTakenIndicator damageTakenIndicator)
        {
            Helper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.gameObject.SetActive(false);

            _damageTakenIndicator = damageTakenIndicator;
            previousHealthProportion = 1.0f;
        }

        public HealthDial(Image healthDialImage, IFilter<TDamagable> visibilityFilter)
        {
            Helper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.gameObject.SetActive(false);
        }

        private void _damagable_HealthChanged(object sender, EventArgs e)
        {
            UpdateDial();
        }

        private void UpdateDial()
        {
            float proportionOfMaxHealth = _damagable.Health / _damagable.MaxHealth;
            _healthDialImage.fillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.UpdateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }
    }
}