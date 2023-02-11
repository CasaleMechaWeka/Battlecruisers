using BattleCruisers.Data;
using BattleCruisers.Effects.Explosions;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathPerformanceTestGod : MonoBehaviour
    {
        public GameObject particleSystemParentA;
        public GameObject particleSystemParentB;

        private ParticleSystem[] particleSystemsA;
        private ParticleSystem[] particleSystemsB;

        void Start()
        {
            CruiserDeathExplosion[] cruiserDeaths = FindObjectsOfType<CruiserDeathExplosion>();

            foreach (CruiserDeathExplosion death in cruiserDeaths)
            {
                IExplosion deathExplosion = death.Initialise(ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager);
                deathExplosion.Activate(death.Position);
            }

            particleSystemsA = particleSystemParentA.GetComponentsInChildren<ParticleSystem>();
            particleSystemsB = particleSystemParentB.GetComponentsInChildren<ParticleSystem>();
        }

        public void StopAllParticles()
        {
            foreach (ParticleSystem ps in particleSystemsA)
            {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            foreach (ParticleSystem ps in particleSystemsB)
            {
                ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }


        public void RestartParticlesForParentA()
        {
            particleSystemParentA.SetActive(true);
            foreach (ParticleSystem ps in particleSystemsA)
            {
                ps.Play();
            }
        }

        public void RestartParticlesForParentB()
        {
            particleSystemParentB.SetActive(true);
            foreach (ParticleSystem ps in particleSystemsB)
            {
                ps.Play();
            }
        }

    }
}
