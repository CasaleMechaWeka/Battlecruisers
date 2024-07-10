using UnityEngine;

public class BuildableClickAndDrag : MonoBehaviour
{
    private bool _clickAndDraging;
    public bool ClickAndDraging
    {
        get => _clickAndDraging;
        set => _clickAndDraging = value;
    }
}
