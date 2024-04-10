using System.Diagnostics;
using UnityEngine.Events;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class GrapheneSectorShieldController : SectorShieldController
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
