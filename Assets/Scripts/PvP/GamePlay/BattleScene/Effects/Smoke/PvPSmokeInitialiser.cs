using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeInitialiser : MonoBehaviour
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private PvPSmokeEmitter _smokeEmitter;
#pragma warning restore CS0414  // Variable is assigned but never used

        public void Initialise(PvPTarget parentDamagable, bool showSmokeWhenDestroyed)
        {
            PvPSmoke smoke = GetComponent<PvPSmoke>();
            Assert.IsNotNull(smoke);
            smoke.Initialise(new PvPSmokeChanger());

            _smokeEmitter
                = new PvPSmokeEmitter(
                    new PvPHealthStateMonitor(parentDamagable),
                    smoke,
                    showSmokeWhenDestroyed);
        }
    }
}