using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Fetchers;
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

        protected override void DestroyCruiser(Helper helper, Cruiser cruiser)
        {
            Assert.IsNotNull(deferrer);
            Assert.IsNotNull(deathPrefab);

            deferrer.Defer(() =>
            {
                Destroy(cruiser.gameObject);

                ShowProjectileExplosion(helper, explosionPosition);

                GameObject deathInstance = Instantiate(deathPrefab);
                deathInstance.transform.position = cruiser.Position;
            },
            delayInS: deathTimeInS);
        }

        private void ShowProjectileExplosion(Helper helper, Vector2 explosionPosition)
        {
            BuildableInitialisationArgs initialisationArgs = new BuildableInitialisationArgs(helper);

            if (showArtilleryExplosion)
            {
                PrefabFactory.ShowExplosion(ExplosionType.Explosion150, explosionPosition);
            }

            if (showNukeExplosion)
            {
                PrefabFactory.ShowExplosion(ExplosionType.Explosion500, explosionPosition);
            }
        }
    }
}