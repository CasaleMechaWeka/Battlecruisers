using BattleCruisers.Effects.Smoke;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
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

        public void Initialise(PvPBuildable<PvPBuildableActivationArgs> parentDamagable, bool showSmokeWhenDestroyed)
        {
            BattleCruisers.Effects.Smoke.Smoke smoke = GetComponent<BattleCruisers.Effects.Smoke.Smoke>();
            Assert.IsNotNull(smoke);
            smoke._particleSystem.Clear();
            smoke.Initialise(new SmokeChanger());

            _smokeEmitter
                = new PvPSmokeEmitter(
                    parentDamagable,
                    new PvPHealthStateMonitor(parentDamagable),
                    smoke,
                    showSmokeWhenDestroyed);
        }

        public void Initialise(PvPBuildable<PvPBuildingActivationArgs> parentDamagable, bool showSmokeWhenDestroyed)
        {
            BattleCruisers.Effects.Smoke.Smoke smoke = GetComponent<BattleCruisers.Effects.Smoke.Smoke>();
            Assert.IsNotNull(smoke);
            smoke._particleSystem.Clear();
            smoke.Initialise(new SmokeChanger());

            _smokeEmitter
                = new PvPSmokeEmitter(
                    parentDamagable,
                    new PvPHealthStateMonitor(parentDamagable),
                    smoke,
                    showSmokeWhenDestroyed);
        }
    }
}