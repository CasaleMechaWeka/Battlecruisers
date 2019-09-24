using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTestGod : MonoBehaviour
    {
        void Start()
        {
            CreateAndDestroyCruiser();
        }

        private void CreateAndDestroyCruiser()
        {
            Cruiser cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(cruiser);
            Helper.SetupCruiser(cruiser);
            DestroyCruiser(cruiser);
        }

        protected virtual void DestroyCruiser(Cruiser cruiser)
        {
            cruiser.Destroy();
        }
    }
}