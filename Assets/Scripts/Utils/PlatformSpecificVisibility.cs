using UnityEngine;

public class PlatformSpecificVisibility : MonoBehaviour
{
    void Start()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        // Disable this GameObject if it's running on Windows
        gameObject.SetActive(false);
#endif
    }
}
