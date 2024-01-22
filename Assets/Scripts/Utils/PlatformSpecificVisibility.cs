using UnityEngine;

public class PlatformSpecificVisibility : MonoBehaviour
{
    void Start()
    {
#if UNITY_STANDALONE_WIN
        // Disable this GameObject if it's running on Windows
        gameObject.SetActive(false);
#endif
    }
}
