// FELIX  Remove :D
namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    /// <summary>
    /// Uses a subset of an underlying fillable image.  For example,
    /// with an underlying fillable image spanning 180*, and minimum
    /// and maximum proportions of 0.25 and 0.75 respectively, setting
    /// the FillAmount to:  Results in:
    ///                 0   => 0.25
    ///                 0.5 => 0.5
    ///                 1   => 0.75
    /// </summary>
    public class SubsetFillableImage : IFillableImage
    {
        private readonly IFillableImage _baseIamge;
        private readonly IFillCalculator _fillCalculator;

        public bool IsVisible
        {
            get { return _baseIamge.IsVisible; }
            set { _baseIamge.IsVisible = value; }
        }

        public float FillAmount
        {
            get { return _fillCalculator.AdjustedToRaw(_baseIamge.FillAmount); }
            set { _baseIamge.FillAmount = _fillCalculator.RawToAdjusted(value); }
        }

        public SubsetFillableImage(IFillableImage baseImage, IFillCalculator fillCalculator)
        {
            Helper.AssertIsNotNull(baseImage, fillCalculator);

            _baseIamge = baseImage;
            _fillCalculator = fillCalculator;
        }
    }
}