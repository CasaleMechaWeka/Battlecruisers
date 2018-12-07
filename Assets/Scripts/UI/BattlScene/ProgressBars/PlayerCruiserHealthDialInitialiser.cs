using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class PlayerCruiserHealthDialInitialiser : MonoBehaviour
    {
        private const float MIN_PROPORTION = 0.12f;
        private const float MAX_PROPORTION = 0.77f;

        public IMaskHighlightable Initialise(ICruiser playerCruiser)
        {
            Assert.IsNotNull(playerCruiser);

            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IFillableImage coreFillableImage = new FillableImage(platformFillableImage);
            IFillCalculator fillCalculator = new FillCalculator(MIN_PROPORTION, MAX_PROPORTION);
            IFillableImage subsetFillableImage = new SubsetFillableImage(coreFillableImage, fillCalculator);

            IFilter<ICruiser> visibilityFilter = new StaticFilter<ICruiser>(isMatch: true);

            IHealthDial<ICruiser> healthDial = new HealthDial<ICruiser>(subsetFillableImage, visibilityFilter);
            healthDial.Damagable = playerCruiser;

            MaskHighlightable highlightable = GetComponent<MaskHighlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }
    }
}