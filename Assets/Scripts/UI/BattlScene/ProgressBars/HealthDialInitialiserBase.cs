using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public abstract class HealthDialInitialiserBase<TDamagable> : MonoBehaviour where TDamagable : IDamagable
    {
        public IHealthDial<TDamagable> Initialise()
        {
            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IFillableImage fillableImage = new FillableImage(platformFillableImage);

            IFilter<TDamagable> visibilityFilter = CreateVisibilityFilter();

            return new HealthDial<TDamagable>(fillableImage, visibilityFilter);
        }

        protected abstract IFilter<TDamagable> CreateVisibilityFilter();
    }
}