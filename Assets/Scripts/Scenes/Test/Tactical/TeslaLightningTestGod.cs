using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class TeslaLightningTestGod : MonoBehaviour
    {
        public LightningBoltScript lightningBolt;
        public float fireIntervalInS = 1;

        void Start()
        {
            Assert.IsNotNull(lightningBolt);
            InvokeRepeating(nameof(Fire), time: 0, repeatRate: fireIntervalInS);
        }

        private void Fire()
        {
            lightningBolt.Trigger();
        }
    }
}