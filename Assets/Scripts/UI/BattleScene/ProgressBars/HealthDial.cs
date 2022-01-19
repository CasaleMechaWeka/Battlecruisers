using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class HealthDial<TDamagable> : IHealthDial<TDamagable> where TDamagable : IDamagable
    {
        private readonly IFillableImage _healthDialImage;
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
                    _healthDialImage.IsVisible = false;
                }

                _damagable = value;

                if (_damagable != null
                    && _visibilityFilter.IsMatch(_damagable))
                {
                    _healthDialImage.IsVisible = true;
                    UpdateDial();
                    _damagable.HealthChanged += _damagable_HealthChanged;
                }
            }
        }

        public HealthDial(IFillableImage healthDialImage, IFilter<TDamagable> visibilityFilter, DamageTakenIndicator damageTakenIndicator)
        {
            Helper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.IsVisible = false;

            _damageTakenIndicator = damageTakenIndicator;
            _damageTakenIndicator.initialise();
            previousHealthProportion = 1.0f;
        }

        public HealthDial(IFillableImage healthDialImage, IFilter<TDamagable> visibilityFilter)
        {
            Helper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.IsVisible = false;
        }

        private void _damagable_HealthChanged(object sender, EventArgs e)
        {
            UpdateDial();
        }

        private void UpdateDial()
        {
            float proportionOfMaxHealth = _damagable.Health / _damagable.MaxHealth;
            _healthDialImage.FillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.updateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }
    }
}