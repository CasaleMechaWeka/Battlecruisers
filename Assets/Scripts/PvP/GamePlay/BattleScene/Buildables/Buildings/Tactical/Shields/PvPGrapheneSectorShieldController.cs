using UnityEngine.Events;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{

    public class PvPGrapheneSectorShieldController : PvPSectorShieldController
    {
        public UnityEvent onShieldDepleted;
        public UnityEvent onShieldDamaged;


        protected override void OnHealthGone()
        {
            base.OnHealthGone();
            onShieldDepleted.Invoke();
        }

        protected override void OnTakeDamage()
        {
            base.OnTakeDamage();
            onShieldDamaged.Invoke();
        }


        public void SetShieldHealth(float newHealth)
        {
            _healthTracker.SetHealth(newHealth);
        }
    }
}