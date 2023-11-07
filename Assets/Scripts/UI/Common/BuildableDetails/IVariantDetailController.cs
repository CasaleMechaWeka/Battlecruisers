using UnityEngine.UI;
namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IVariantDetailController
    {
        Text variantName { get; set; }
        Text variantDescription { get; set; }
        Image variantIcon { get; set; }
    }
}

