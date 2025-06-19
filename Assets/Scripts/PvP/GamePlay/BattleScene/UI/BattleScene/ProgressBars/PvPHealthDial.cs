using UnityEngine.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public class PvPHealthDial
    {
        private readonly Image _healthDialImage;
        private readonly IFilter<PvPTarget> _visibilityFilter;
        private DamageTakenIndicator _damageTakenIndicator;

        private float previousHealthProportion;

        private PvPTarget _damagable;
        public PvPTarget Damagable
        {
            set
            {
                if (_damagable != null)
                {
                    _damagable.pvp_Health.OnValueChanged -= _damagable_HealthChanged;
                    _healthDialImage.gameObject.SetActive(false);
                }

                _damagable = value;

                if (_damagable != null
                    && _visibilityFilter.IsMatch(_damagable))
                {
                    _healthDialImage.gameObject.SetActive(true);
                    UpdateDial();
                    _damagable.pvp_Health.OnValueChanged += _damagable_HealthChanged;
                }
            }
        }

        public PvPHealthDial(Image healthDialImage, IFilter<PvPTarget> visibilityFilter, DamageTakenIndicator damageTakenIndicator)
        {
            PvPHelper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.gameObject.SetActive(false);

            _damageTakenIndicator = damageTakenIndicator;
            previousHealthProportion = 1.0f;
        }

        public PvPHealthDial(Image healthDialImage, IFilter<PvPTarget> visibilityFilter)
        {
            PvPHelper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;
            _healthDialImage.gameObject.SetActive(false);
        }

        private void _damagable_HealthChanged(float oldVal, float newVal)
        {
            UpdateDial(newVal);
        }

        private void UpdateDial(float curHealth)
        {
            float proportionOfMaxHealth = curHealth / _damagable.MaxHealth;
            _healthDialImage.fillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.UpdateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }


        private void UpdateDial()
        {
            float proportionOfMaxHealth = _damagable.pvp_Health.Value / _damagable.MaxHealth;
            _healthDialImage.fillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.UpdateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }
    }
}