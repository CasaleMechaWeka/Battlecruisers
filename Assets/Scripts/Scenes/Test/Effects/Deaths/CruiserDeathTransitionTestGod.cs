using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTransitionTestGod : CruiserDeathTestGod
    {
        public TimeScaleDeferrer deferrer;
        public float deathTimeInS = 1;
        public GameObject deathPrefab;
        public bool showArtilleryExplosion = true;
        public bool showNukeExplosion = false;
        public Vector2 explosionPosition = new Vector2(0, 1);

        protected override void DestroyCruiser(Cruiser cruiser)
        {
            Assert.IsNotNull(deferrer);
            Assert.IsNotNull(deathPrefab);

            deferrer.Defer(() =>
            {
                Destroy(cruiser.gameObject);

                ShowProjectileExplosion(explosionPosition);

                GameObject deathInstance = Instantiate(deathPrefab);
                deathInstance.transform.position = cruiser.Position;
            }, 
            delayInS: deathTimeInS);
        }

        private void ShowProjectileExplosion(Vector2 explosionPosition)
        {
            BuildableInitialisationArgs initialisationArgs = new BuildableInitialisationArgs(new Helper());

            if (showArtilleryExplosion)
            {
                initialisationArgs.FactoryProvider.PoolProviders.ExplosionPoolProvider.LargeExplosionsPool.GetItem(explosionPosition);
            }

            if (showNukeExplosion)
            {
                initialisationArgs.FactoryProvider.PoolProviders.ExplosionPoolProvider.HugeExplosionsPool.GetItem(explosionPosition);
            }
        }
    }
}